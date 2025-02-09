import taskCardServices from "@/api/taskCard-service"
import { CreateTaskCardData, TaskCardResponse } from "@/schemas/task-card.ts"
import { ApiError } from "@/utils/errors"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"

export function useTaskCards(listId?: string) {
    return useQuery<TaskCardResponse[], ApiError>({
        queryKey: ["taskCards", listId],
        queryFn: () => {
            if (!listId) {
                throw new Error("listId is required")
            }
            return taskCardServices.getByList(listId)
        },
        enabled: !!listId
    })
}

export function useCreateTaskCard() {
    const queryClient = useQueryClient()

    return useMutation<TaskCardResponse, ApiError, CreateTaskCardData>({
        mutationFn: taskCardServices.addOne,
        onSuccess: () => {
            // Invalidate the lists query since tasks are nested in lists
            queryClient.invalidateQueries({
                queryKey: ["lists"]
            })
        },
        retry: (failureCount: number, error: ApiError) => {
            const noRetryConditions = [
                error.message.includes("already exists"),
                error.statusCode === 429,
                error.statusCode === 400,
                error.statusCode === 422,
                error.statusCode === 403,
                error.statusCode === 404,
                error.statusCode === 500
            ]

            if (noRetryConditions.some((condition) => condition)) {
                return false
            }
            return failureCount < 3
        },
        retryDelay: (attemptIndex) => Math.min(1000 * 2 ** attemptIndex, 30000)
    })
}

export function useUpdateTaskCardPositions() {
    const queryClient = useQueryClient()

    return useMutation<void, ApiError, { taskCardId: string; listId: string; position: number }[]>({
        mutationFn: taskCardServices.updateMultiplePositions,
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: ["lists"] })
        }
    })
}
