# Table of Contents
- [Table of Contents](#table-of-contents)
  - [Intro](#intro)
  - [Tech Overview](#tech-overview)
  - [Techniques](#techniques)
    - [Data flow](#data-flow)
    - [Lazy loading](#lazy-loading)
  - [Running the application](#running-the-application)
  - [Environment variables](#environment-variables)
  - [What to expect](#what-to-expect)
  - [Improvements and future features](#improvements-and-future-features)


## Intro
This a task management application aiming to help users organize their projects and teams with their realated task in specified workspaces. A user can careate and be part of multiple worksapces, create multiple projetcs and add members to the workspsace (if they are an owner or and admin of the worksapce). Members of the workspace can then create Kanaban boards for a specific project. This fornt-end implementation is built within 3 weeks by the time of writing this documentation 02.01.2025-23.01.2025. 

## Tech Overview
 - **React** was the chosen forntend libary to facilitate implementation by deviding pages into smaller components
 - This **React** app is written in **TypeScript** to insure type saftey and prevent runtime errors
 - Since Kanban-board is one of the main feautres, **DND-kit** is used to implement dragging and dropping of columns as well as their respective tasks
 - **TanStack React Query** is used for handeling data fetching and mutations as well as utilizing caching and query invalidation to insure UI updates

## Techniques
The project tries to follow the "seperation of concerns" prenciple to decopule UI from the business logic for better testability and scalability.

### Data flow
The data flow is managed through custome hooks, which incapsulate the business logic. Within the hooks `React-Query` handles fetching or mutating by calling the endpoint as a mutation or query function, then the 'onSuccess or onError' are handled after. The endpoints are called from a seperate service file with axios. Example:
`handleSubmit` function for creating a task card within a Kanban board column:

```TS
const createTask = useCreateTaskCard()
const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault()
        if (title.trim() && listId) {
            const newTask: CreateTaskCardData = {
                title: title.trim(),
                description: description.trim(),
                listId,
                position: lists.find(list => list.id === listId)?.taskCard?.length || 0,
            }
            createTask.mutate(newTask, {
                onSuccess: () => {
                    setTitle('')
                    setDescription('')
                    setListId('')
                    setOpen(false)
                },
                onError: (error) => {
                    console.error('Error creating task:', error)
                    toast.error('Failed to create task. Please try again.')
                }
            })
        }
    }

```
`useCreatetaskCard()`:
```TS
export function useCreateTaskCard() {
    const queryClient = useQueryClient()
    
    return useMutation<TaskCardResponse, ApiError, CreateTaskCardData>({
        mutationFn: taskCardServices.addOne,
        onSuccess: () => {
            // Invalidate the lists query since tasks are nested in lists
            queryClient.invalidateQueries({ 
                queryKey: ['lists']
            })
        },
        retry: (failureCount: number, error: ApiError) => {
            const noRetryConditions = [
                error.message.includes('already exists'),  
                error.statusCode === 429,  
                error.statusCode === 400, 
                error.statusCode === 422, 
                error.statusCode === 403,
                error.statusCode === 404,
                error.statusCode === 500
            ]

            if (noRetryConditions.some(condition => condition)) {
                return false
            }
            return failureCount < 3
        },
        retryDelay: (attemptIndex) => Math.min(1000 * (2 ** attemptIndex), 30000),
    })
}
```
`Task card service`
```TS
const taskCard: TaskCardServices = {
    addOne: async (taskCardData) => {
        const response = await api.post("/TaskCards", taskCardData)
        const validatedData = taskCardResponseSchema.safeParse(response.data.data)

        if (!validatedData.success) {
            throw new ApiError("Invalid data", 400)
        }

        return validatedData.data
    },
}
```
### Lazy loading
Lazy loading is implemented to improve performance, by only loading the necessary parts (pages) when needed instead of sending all pages to the end-users at one go. It requires default exports and suspense.

```TS
import { Suspense, lazy } from "react"

const ProjectBoardPage = lazy(() => import("./pages/dashboard/kanban/projectboard.tsx"))
const Profile = lazy(() => import("./pages/dashboard/profile/profile.tsx"))
const Team = lazy(() => import("./pages/dashboard/team/team.tsx"))

....

 <Route
      element={
          <Layout>
              <Suspense fallback={<div>Loading...</div>}>
                  <ProjectBoardPage />
              </Suspense>
          </Layout>
      }
      path="kanban"
  />
```

## Running the application
Simply after cloning the repo or downloading the project, open the terminal and run:

```sh
CD your-project-dir # moves to project directory

yarn # downloads all the necessary packages

yarn dev # Runs the app on localhost
```

## Environment variables

Below are the environment variables required for the application:

| Environment Variable | Description                       |
| -------------------- | --------------------------------- |
| `VITE_API_URL_LOCAL` | http://localhost:5140/api/v1      |
| `VITE_API_URL`       | http://yourdeployedbackend/api/v1 |


## What to expect
- Ability to register and login
- Create workspaces and view information about it
- Create projects and see all the workspace related projects
- Add new members to the current workspace (as an admin or owner) and view all the members of the current workspace
- While searching for a user by email, if it exists in the database it will show as a 'suggestion' under the search input field
- Create Kanban boards and tasks for a selected project within the workspace
- Ability to reorder the columns created and move tasks to different columns by dragging and dropping
- Columns and task cards positions are presisted in the database and upadated after a drag and drop action

## Improvements and future features
- Improve Error handling and error messages
- Implement optimistic rendering
- Implement all the crud operations
- Integrate the rest of the backend endpoints
- Writing tests

