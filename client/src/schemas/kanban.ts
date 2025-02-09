export interface AppList {
    id: string;
    name: string;
    position: number;
    projectBoardId: string;
    taskCards: TaskCard[];
}
  
export interface CreateAppList {
    name: string;
    projectBoardId: string;
    position: number;
}
  
export interface UpdateAppList {
    name?: string;
    position?: number;
}
  
export interface UpdateListPositions {
    listPositions: {
        listId: string;
        newPosition: number;
    }[];
}
  
export interface TaskCard {
    id: string;
    title: string;
    description: string;
    listId: string;
    position: number;
    createdAt: string;
    updatedAt: string;
    priorityId: string;
    statusId: string;
    dueDate?: string;
    activities: TaskCardActivity[];
    status: Status;
    labels: ProjectBoardLabel[];
}
  
export interface CreateTaskCard {
    title: string;
    description: string;
    listId: string;
    position: number;
    priorityId: string;
    statusId: string;
    dueDate?: string;
}
  
export interface UpdateTaskCard {
    title?: string;
    description?: string;
    listId?: string;
    position?: number;
    priorityId?: string;
    statusId?: string;
    dueDate?: string;
}
  
export interface TaskCardPositionUpdate {
    taskCardId: string;
    listId: string;
    position: number;
}
  
export interface TaskCardActivity {
    id: string;
    description: string;
    createdAt: string;
    taskCardId: string;
}
  
export interface Status {
    id: string;
    name: string;
}
  
export interface ProjectBoardLabel {
    id: string;
    name: string;
    color: string;
    projectBoardId: string;
}
  
export interface Priority {
    id: string;
    name: string;
    level: number;
}