#### Use Cases

1. Create Project
2. Create Developer
3. View Project
4. View Developer
5. Edit Project
6. Edit Developer
7. Assign Developers to projects
8. Unassign devlopers from projects
9.? Delete projects/developers
10. View Active Projects
11. View assigned developers for current project
12. View Completed projects
13. View all developers
14. View all project
15. View upcomming projects
16. Search developer
17. Search project

 #### Implementation queue & Application Facase

| Use case|Status| Manager |Status|Component|Status
|-| - |-| - |-|-|
| Create Project | pre | Add\<T>(T x)| done | create-project-| pre |
| Create Developer| pre | Add\<T>(T x)| done | create-developer- | done |
| View Project| pre | Get\<T>(...)| done | project- | done |
| View Developer| pre | Get\<T>(...)| pre | developer- | unstarted |
| View Active Projects| unstrated | GetActiveProjects(...)| unstarted | active-projects  | unstarted |
| View all developers| unstrated | Get\<T>(...)| pre |all-developers,list-developers| unstarted |
| View all project| unstrated | Get\<T>(...)| pre | all-projects,list-developers | unstarted |
| Edit Project| unstrated | Update\<T>(T x)| done | edit-project | unstarted |
| Edit Developer| unstrated | Update\<T>(T x)| done | edit-developer | unstarted |
| View Completed projects| unstrated | GetProjects(...)| unstrated | all-projects | unstarted |
| View upcomming projects| unstrated | GetProjects(...)| unstrated | all-projects | unstarted |
| Delete projects/developers| unstrated | Remove\<T>(T x)| unstrated | edit-developer | unstarted |
| Assign Developers to projects| unstrated | Assign(proj,dev) // try | unstrated | ? | unstarted |
| Unassign devlopers from projects| unstrated | Unassign(proj,dev) //try | unstrated | ? | unstarted |
| View assigned developers for current project| unstrated | GetDevelopers(...)| unstrated | view-develoeprs | unstarted |
| Search developer| unstrated | Get\<T>(...keys)| pre | search-component | unstarted |
| Search project| unstrated | Get\<T>(...keys)| pre| search-component | unstarted |


