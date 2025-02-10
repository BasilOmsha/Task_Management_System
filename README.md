# Task_Management_System# Table of Contents

- [Task\_Management\_System# Table of Contents](#task_management_system-table-of-contents)
  - [Intro](#intro)
  - [Getting Started](#getting-started)
  - [Running the app locally](#running-the-app-locally)
  - [Known Issues in the Application](#known-issues-in-the-application)
  - [What to expect](#what-to-expect)
  - [Future Improvements](#future-improvements)
  
## Intro
This project aim to build a task management system to help users create worksace, projects and orgnize related tasks in a kanban board. In addition users can add colaborators.
Note the back-end was built within 3 weeks as well as the front-end. 

## Getting Started

Clone the repository to your machine:

```sh
$ git clone https://github.com/BasilOmsha/Task_Management_System.git
```

## Running the app locally
Please refer to the readme file first within the server directory and then check the one inside the client directory.

## Known Issues in the Application
- Not all CRUD operation implemened.
- Delay when when moving tasks betwen the kanban "columns". Example, from "ToDo" to "Done." Possible fix optimistic rendering.
- Refreshing the page while logged in, will take you back to the dashboard.

## What to expect
- Ability to sign up
- Ability to login 
- Validation Errors
- Ability to view own user's info when logged in
- Ability to create workspace, projects, add users to workspaces and create kanban board with task.
- Ability to rearrange tasks and columns in the kanban board
- Ability to sign out

## Future Improvements
- Improve Error handling in the backend and error messages
- Implement all the CRUD operations
- Fix the refreshing bug
- Implement roles and permisions
- Implement optimistic redering
- Writing tests specially for the fornt-end
