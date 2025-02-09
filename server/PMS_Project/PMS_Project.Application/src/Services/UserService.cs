using PMS_Project.Application.Abstractions.Services;
using PMS_Project.Application.DTOs;
using PMS_Project.Domain.Models;
using AutoMapper;
using PMS_Project.Domain.Abstractions.Repositories;

namespace PMS_Project.Application.Services
{
    public class UserService : IUserService, IBaseService<CreateUserDTO, GetUserInfoDTO, UpdateUserInfoDTO> //IUserService already inherits from IBaseService, so no need to inherit from IBaseService again
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IPaswdService _paswdService;
        //private ITaskCardRepository _taskCardRepository;

        public UserService(IUserRepository userRepo, IMapper mapper, IPaswdService PaswdService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _paswdService = PaswdService;
        }

        public async Task<GetUserInfoDTO> AddAsync(CreateUserDTO createDTO)
        {
            var user = _mapper.Map<User>(createDTO);
            var emailExists = await CheckUserEmailExistsAsync(user.Email!);
            var usernameExists = await CheckUserUsernameExistsAsync(user.Username!);
            if (emailExists == true)
            {
                throw new Exception("Email already exists");
            }
            if (usernameExists == true)
            {
                throw new Exception("Username already exists");
            }

            var hashedPaswd = _paswdService.HashPaswd(createDTO.Password!);
            // Check if confirm password matches the password
            var verifyPaswd = _paswdService.VerifyPaswd(createDTO.ConfirmPassword!, hashedPaswd);
            if (!verifyPaswd)
            {
                throw new Exception("Password and confirm password do not match.");
            }
            user.Password = hashedPaswd;
            // Set CreatedAt and UpdatedAt
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;

            var addedUser = await _userRepo.AddAsync(user);
            return _mapper.Map<GetUserInfoDTO>(addedUser);
        }

        public async Task<GetUserInfoDTO> GetByIdAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id) ?? throw new Exception("User not found.");
            var mappedUser = _mapper.Map<GetUserInfoDTO>(user);
            return mappedUser;

        }
        public async Task<GetUserInfoDTO> GetUserByEmailAsync(string email)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            if (user == null)
            {
                // throw new Exception("User not found.");
                return null;
            }
            return _mapper.Map<GetUserInfoDTO>(user);
        }

        public async Task<bool> CheckUserEmailExistsAsync(string email)
        {
            if (await _userRepo.GetUserByEmailAsync(email) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckUserUsernameExistsAsync(string username)
        {
            if (await _userRepo.GetUserByUserNameAsync(username) != null)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<GetUserInfoDTO>> GetAllAsync()
        {
            var users = await _userRepo.GetAllAsync();
            foreach (var user in users)
            {
                if (user == null)
                {
                    throw new ArgumentNullException(nameof(user));
                }
            }
            return _mapper.Map<IEnumerable<GetUserInfoDTO>>(users);
        }

        public async Task<GetUserInfoDTO> UpdateAsync(Guid id, UpdateUserInfoDTO updateDTO)
        {
            if (updateDTO == null)
            {
                throw new ArgumentNullException(nameof(updateDTO), "Update data must be provided.");
            }

            // Retrieve the user by ID 
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }


            // Update user properties if they are provided in the updateDTO
            if (!string.IsNullOrEmpty(updateDTO.FirstName))
                user.Firstname = updateDTO.FirstName;

            if (!string.IsNullOrEmpty(updateDTO.LastName))
                user.Lastname = updateDTO.LastName;

            if (!string.IsNullOrEmpty(updateDTO.Username))
            {
                // check if the username already exists
                var usernameExists = await CheckUserUsernameExistsAsync(updateDTO.Username);
                if (usernameExists)
                {
                   if (user.Username == updateDTO.Username)
                    {
                        user.Username = updateDTO.Username;
                    }
                    else
                    {
                        throw new Exception("Username already exists.");
                    }
                }
                user.Username = updateDTO.Username;
            }

            if (!string.IsNullOrEmpty(updateDTO.Email))
            {
                // check if the email already exists
                var emailExists = await CheckUserEmailExistsAsync(updateDTO.Email);
                if (emailExists && user.Email != updateDTO.Email)
                {
                    throw new Exception("Email already exists.");
                }
                user.Email = updateDTO.Email;
            }

            // verify current password is correct
            var verifyPaswd = _paswdService.VerifyPaswd(updateDTO.CurrentPassword!, user.Password!);
            if (!verifyPaswd)
            {
                throw new Exception("Current password is incorrect.");
            }
            // verify new password and confirm password match
            var hashedPaswd = _paswdService.HashPaswd(updateDTO.NewPassword!);
            var verifyNewPaswd = _paswdService.VerifyPaswd(updateDTO.ConfirmNewPassword!, hashedPaswd);
            if (!verifyNewPaswd)
            {
                throw new Exception("New password and confirm password do not match.");
            }
            user.Password = hashedPaswd;

            // Update the UpdatedAt timestamp
            user.UpdatedAt = DateTime.UtcNow;

            // Save changes to the repository
            var updatedUser = await _userRepo.UpdateAsync(user);

            // Map the updated User entity to GetUserInfoDTO
            var userInfoDTO = _mapper.Map<GetUserInfoDTO>(updatedUser);
            return userInfoDTO;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            return await _userRepo.DeleteAsync(id);
        }

        public async Task AddTaskCardActivityAsync(Guid userId, TaskCardActivity activity)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            user.TaskCardActivities?.Add(activity);
            await _userRepo.UpdateAsync(user);
        }

        public async Task RemoveTaskCardActivityAsync(Guid userId, TaskCardActivity activity)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            user.TaskCardActivities?.Remove(activity);
            await _userRepo.UpdateAsync(user);
        }

       
    }
}