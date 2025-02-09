import listServices from "@/api/list-service.ts"
import { ListData, ListResponse } from "@/schemas/list"
import { ApiError } from "@/utils/errors"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"

export function useLists(projectBoardId?: string) {
    return useQuery<ListResponse[], ApiError>({
        queryKey: ['lists', projectBoardId],
        queryFn: async () => {
            if (!projectBoardId) {
                throw new Error("projectBoardId is required")
            }
            try {
                const result = await listServices.getByProjectBoard(projectBoardId)
                console.log('Lists query result:', result)
                return result
            } catch (error) {
                console.error('Lists query error:', error)
                throw error
            }
        },
        enabled: !!projectBoardId
    })
}

export function useCreateList() {
    const queryClient = useQueryClient()
    
    return useMutation<ListResponse, ApiError, ListData>({
        mutationFn: async (listData) => {
            try {
                const result = await listServices.addOne(listData)
                console.log('Mutation result:', result)
                return result
            } catch (error) {
                console.error('Mutation error:', error)
                throw error
            }
        },
        onSuccess: (data) => {
            console.log('Mutation success, invalidating queries')
            queryClient.invalidateQueries({ 
                queryKey: ['lists', data.projectBoardId] 
            })
        },
        onError: (error) => {
            console.error('Mutation error in hook:', error)
        }
    })
}

export function useUpdateListPositions() {
    const queryClient = useQueryClient()
    
    return useMutation<void, ApiError, { listId: string; newPosition: number }[]>({
        mutationFn: listServices.updateMultiplePositions,
        onSettled: () => {
            queryClient.invalidateQueries({ queryKey: ['lists'] })
        }
    })
}