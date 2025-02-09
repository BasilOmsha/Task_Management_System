using PMS_Project.Domain.Abstractions.Repositories;
using PMS_Project.Domain.Models;
using PMS_Project.Application.DTOs;
using PMS_Project.Application.Abstractions.Services;
using AutoMapper;

namespace PMS_Project.Application.Services
{
    public class TaskCardService : ITaskCardService, IBaseService<CreateTaskCardDTO, TaskCardDTO, TaskCardDTO>
    {
        private readonly ITaskCardRepository _taskCardRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        private readonly IStatusRepository _statusRepository;
        private readonly IPriorityRepository _priorityRepository;

        public TaskCardService(ITaskCardRepository taskCardRepository, IMapper mapper, IUserRepository userRepository, IStatusRepository statusRepository,
        IPriorityRepository priorityRepository)
        {
            _taskCardRepository = taskCardRepository;
            _mapper = mapper;
            //_taskCardRepository = taskCardRepository;
            _userRepository = userRepository;
            _statusRepository = statusRepository;
            _priorityRepository = priorityRepository;
        }

        public async Task<TaskCardDTO> GetByIdAsync(Guid id)
        {
            var taskCard = await _taskCardRepository.GetByIdAsync(id);
            return new TaskCardDTO
            {
                Id = taskCard.Id,
                Description = taskCard.Description,
                ListId = taskCard.ListId,
                CreatedAt = taskCard.CreatedAt,
                UpdatedAt = taskCard.UpdatedAt,
                Title = taskCard.Title,
                PriorityId = taskCard.PriorityId,
                Activities = _mapper.Map<ICollection<TaskCardActivityDTO>>(taskCard.Activities),
                StatusId = taskCard.StatusId
            };
        }

        public async Task<IEnumerable<TaskCardDTO>> GetAllAsync()
        {
            var taskCards = await _taskCardRepository.GetAllAsync();
            return taskCards.Select(tc => new TaskCardDTO
            {
                Id = tc.Id,
                Description = tc.Description,
                ListId = tc.ListId,
                CreatedAt = tc.CreatedAt,
                UpdatedAt = tc.UpdatedAt,
                Title = tc.Title,
                PriorityId = tc.PriorityId,
                DueDate = tc.DueDate,
                StatusId = tc.StatusId,
                Activities = _mapper.Map<ICollection<TaskCardActivityDTO>>(tc.Activities)
            });
        }

        // public async Task<TaskCardDTO> AddAsync(TaskCardDTO taskCardDTO)
        // {
        //     var taskCard = new TaskCard
        //     {
        //         Id = taskCardDTO.Id,
        //         Description = taskCardDTO.Description,
        //         ListId = taskCardDTO.ListId,
        //         CreatedAt = taskCardDTO.CreatedAt,
        //         UpdatedAt = taskCardDTO.UpdatedAt,
        //         Title = taskCardDTO.Title,
        //         PriorityId = taskCardDTO.PriorityId,
        //         DueDate = taskCardDTO.DueDate,
        //         StatusId = taskCardDTO.StatusId
        //     };
        //     await _taskCardRepository.AddAsync(taskCard);
        //     return taskCardDTO;
        // }

        public async Task<TaskCardDTO> UpdateAsync(Guid id, TaskCardDTO taskCardDTO)
        {
            var taskCard = new TaskCard
            {
                Id = taskCardDTO.Id,
                Description = taskCardDTO.Description,
                ListId = taskCardDTO.ListId,
                CreatedAt = taskCardDTO.CreatedAt,
                UpdatedAt = taskCardDTO.UpdatedAt,
                Title = taskCardDTO.Title,
                PriorityId = taskCardDTO.PriorityId,
                DueDate = taskCardDTO.DueDate,
                StatusId = taskCardDTO.StatusId,
                Status = taskCardDTO.Status != null ? new Status { Id = taskCardDTO.Status.Id, Name = taskCardDTO.Status.Name } : null
            };
            await _taskCardRepository.UpdateAsync(taskCard);
            return taskCardDTO;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await _taskCardRepository.DeleteAsync(id);
            return true;
        }

        public async Task AddTaskCardActivityAsync(Guid taskCardId, TaskCardActivity activity)
        {
            var taskCard = await _taskCardRepository.GetByIdAsync(taskCardId);
            taskCard.Activities?.Add(activity);
            await _taskCardRepository.UpdateAsync(taskCard);
        }

        public async Task RemoveTaskCardActivityAsync(Guid taskCardId, TaskCardActivity activity)
        {
            var taskCard = await _taskCardRepository.GetByIdAsync(taskCardId);
            taskCard.Activities?.Remove(activity);
            await _taskCardRepository.UpdateAsync(taskCard);
        }

        public async Task AddUserToTaskCardAsync(Guid taskCardId, Guid userId)
        {
            var taskCard = await _taskCardRepository.GetByIdAsync(taskCardId);
            var user = await _userRepository.GetByIdAsync(userId);
            if (taskCard == null || user == null)
            {
                throw new Exception("TaskCard or User not found.");
            }
            var taskCardUser = new TaskCard_User { TaskCardId = taskCardId, UserId = userId };
            taskCard.TaskCard_Users.Add(taskCardUser);
            await _taskCardRepository.AddTaskCardUserAsync(taskCardUser); //populate the joint table
            await _taskCardRepository.UpdateAsync(taskCard);
        }

        public async Task RemoveUserFromTaskCardAsync(Guid taskCardId, Guid userId)
        {
            var taskCard = await _taskCardRepository.GetByIdAsync(taskCardId);
            var user = await _userRepository.GetByIdAsync(userId);
            if (taskCard == null || user == null)
            {
                throw new Exception("TaskCard or User not found.");
            }
            var taskCardUser = taskCard.TaskCard_Users.FirstOrDefault(tcu => tcu.TaskCardId == taskCardId && tcu.UserId == userId);
            if (taskCardUser != null)
            {
                taskCard.TaskCard_Users.Remove(taskCardUser);
                await _taskCardRepository.RemoveTaskCardUserAsync(taskCardId, userId);
                await _taskCardRepository.UpdateAsync(taskCard);
            }
        }

        public async Task<IEnumerable<TaskCardDTO>> GetTaskCardsByListAsync(Guid listId)
        {
            var taskCards = await _taskCardRepository.GetTaskCardsByListAsync(listId);
            return _mapper.Map<IEnumerable<TaskCardDTO>>(taskCards)
                         .OrderBy(tc => tc.Position);
        }

        public async Task UpdateTaskCardPositionAsync(Guid taskCardId, Guid newListId, int newPosition)
        {
            // Add transaction to handle concurrency
            using var transaction = await _taskCardRepository.BeginTransactionAsync();
            try
            {
                var taskCard = await _taskCardRepository.GetByIdAsync(taskCardId);
                if (taskCard == null)
                    throw new KeyNotFoundException($"TaskCard with ID {taskCardId} not found");

                var oldPosition = taskCard.Position;
                var oldListId = taskCard.ListId;
                var cardsToUpdate = new List<TaskCard>();

                // Handle cross-list movement
                if (oldListId != newListId)
                {
                    // Get cards from both source and destination lists
                    var sourceListCards = (await _taskCardRepository.GetTaskCardsByListAsync(oldListId))
                        .OrderBy(tc => tc.Position)
                        .ToList();
                    var destListCards = (await _taskCardRepository.GetTaskCardsByListAsync(newListId))
                        .OrderBy(tc => tc.Position)
                        .ToList();

                    // 1. Update positions in source list (close the gap)
                    foreach (var card in sourceListCards.Where(c => c.Position > oldPosition))
                    {
                        card.Position--;
                        cardsToUpdate.Add(card);
                    }

                    // 2. Update positions in destination list (make space)
                    foreach (var card in destListCards.Where(c => c.Position >= newPosition))
                    {
                        card.Position++;
                        cardsToUpdate.Add(card);
                    }
                }
                else
                {
                    // Same list movement
                    var listCards = (await _taskCardRepository.GetTaskCardsByListAsync(oldListId))
                        .OrderBy(tc => tc.Position)
                        .ToList();

                    if (oldPosition < newPosition)
                    {
                        // Moving down
                        foreach (var card in listCards.Where(c =>
                            c.Id != taskCardId &&
                            c.Position > oldPosition &&
                            c.Position <= newPosition))
                        {
                            card.Position--;
                            cardsToUpdate.Add(card);
                        }
                    }
                    else
                    {
                        // Moving up
                        foreach (var card in listCards.Where(c =>
                            c.Id != taskCardId &&
                            c.Position >= newPosition &&
                            c.Position < oldPosition))
                        {
                            card.Position++;
                            cardsToUpdate.Add(card);
                        }
                    }
                }

                // Update the moved card
                taskCard.Position = newPosition;
                taskCard.ListId = newListId;
                cardsToUpdate.Add(taskCard);

                // Ensure continuous positions (no gaps)
                await EnsureContinuousPositions(oldListId, cardsToUpdate);
                if (oldListId != newListId)
                {
                    await EnsureContinuousPositions(newListId, cardsToUpdate);
                }

                // Update all changes in a single operation
                await _taskCardRepository.UpdateTaskCardPositionsAsync(cardsToUpdate);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task EnsureContinuousPositions(Guid listId, List<TaskCard> cardsToUpdate)
        {
            var listCards = (await _taskCardRepository.GetTaskCardsByListAsync(listId))
                .OrderBy(tc => tc.Position)
                .ToList();

            // Include cards that are already in cardsToUpdate
            var allCards = listCards
                .Where(c => !cardsToUpdate.Any(cu => cu.Id == c.Id))
                .Concat(cardsToUpdate.Where(c => c.ListId == listId))
                .OrderBy(c => c.Position)
                .ToList();

            // Resequence positions to ensure no gaps
            for (int i = 0; i < allCards.Count; i++)
            {
                if (allCards[i].Position != i)
                {
                    allCards[i].Position = i;
                    if (!cardsToUpdate.Any(c => c.Id == allCards[i].Id))
                    {
                        cardsToUpdate.Add(allCards[i]);
                    }
                }
            }
        }

        public async Task UpdateMultipleTaskCardPositionsAsync(IEnumerable<TaskCardPositionUpdateDTO> positionUpdates)
        {
            var updatesByList = positionUpdates.GroupBy(u => u.ListId);
            var cardsToUpdate = new List<TaskCard>();

            foreach (var listGroup in updatesByList)
            {
                var listId = listGroup.Key;
                var currentCards = (await _taskCardRepository.GetTaskCardsByListAsync(listId)).ToList();

                // Update positions sequentially
                var position = 0;
                foreach (var update in listGroup.OrderBy(u => u.Position))
                {
                    var card = await _taskCardRepository.GetByIdAsync(update.TaskCardId);
                    if (card != null)
                    {
                        var oldListId = card.ListId;

                        // If card moved to different list, update old list positions
                        if (oldListId != listId)
                        {
                            var oldListCards = (await _taskCardRepository.GetTaskCardsByListAsync(oldListId))
                                .Where(c => c.Position > card.Position)
                                .OrderBy(c => c.Position)
                                .ToList();

                            foreach (var oldCard in oldListCards)
                            {
                                oldCard.Position = oldCard.Position - 1;
                                cardsToUpdate.Add(oldCard);
                            }
                        }

                        card.ListId = listId;
                        card.Position = position++;
                        cardsToUpdate.Add(card);
                    }
                }

                // Update remaining cards in the target list
                foreach (var existingCard in currentCards.Where(c => !positionUpdates.Any(u => u.TaskCardId == c.Id)))
                {
                    existingCard.Position = position++;
                    cardsToUpdate.Add(existingCard);
                }
            }

            await _taskCardRepository.UpdateTaskCardPositionsAsync(cardsToUpdate);
        }

        public async Task<TaskCardDTO> AddAsync(CreateTaskCardDTO createDTO)
        {
            // Get default status and priority through repositories
            var defaultStatus = await _statusRepository.GetByNameAsync("New");
            var defaultPriority = await _priorityRepository.GetByNameAsync("Low");

            var taskCard = new TaskCard
            {
                Title = createDTO.Title,
                Description = createDTO.Description,
                ListId = createDTO.ListId,
                Position = createDTO.Position,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                StatusId = defaultStatus?.Id ?? Guid.Empty,
                PriorityId = defaultPriority?.Id ?? Guid.Empty,
                DueDate = DateTime.UtcNow.AddDays(7).ToString("yyyy-MM-dd")
            };

            var createdTaskCard = await _taskCardRepository.AddAsync(taskCard);
            return _mapper.Map<TaskCardDTO>(createdTaskCard);
        }

        // public async Task<TaskCardDTO> AddAsync(CreateTaskCardDTO createDTO)
        // {
        //     var taskCard = _mapper.Map<TaskCard>(createDTO);

        //     // Set default status and priority
        //     var defaultStatus = await _statusRepository.GetByNameAsync("New");
        //     var defaultPriority = await _priorityRepository.GetByNameAsync("Low");
        //     taskCard.StatusId = defaultStatus?.Id ?? Guid.Empty;
        //     taskCard.PriorityId = defaultPriority?.Id ?? Guid.Empty;
        //     if (string.IsNullOrEmpty(taskCard.DueDate))
        //     {
        //         taskCard.DueDate = DateTime.UtcNow.AddDays(7).ToString("yyyy-MM-dd"); // Set default due date to 7 days from now
        //     }

        //     var createdTaskCard = await _taskCardRepository.AddAsync(taskCard);
        //     return _mapper.Map<TaskCardDTO>(createdTaskCard);
        // }

    }
}