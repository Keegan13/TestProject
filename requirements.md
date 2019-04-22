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
| Create Project | done | Add\<T>(T x)| done | create-project-| pre |
| Create Developer| done | Add\<T>(T x)| done | create-developer- | done |
| View Project| pre | Get\<T>(...)| done | project- | done |
| View Developer| pre | Get\<T>(...)| pre | developer- | unstarted |
| View Active Projects| pre | GetActiveProjects(...)| unstarted | active-projects  | unstarted |
| View all developers| pre | Get\<T>(...)| pre |all-developers,list-developers| unstarted |
| View all project| pre | Get\<T>(...)| pre | all-projects,list-developers | unstarted |
| Edit Project| done | Update\<T>(T x)| done | edit-project | unstarted |
| Edit Developer| done| Update\<T>(T x)| done | edit-developer | unstarted |
| View Completed | pre | GetProjects(...)| unstrated | all-projects | unstarted |
| View upcomming projects| pre| GetProjects(...)| unstrated | all-projects | unstarted |
| Delete projects/developers| skip? | Remove\<T>(T x)| unstrated | edit-developer | unstarted |
| Assign Developers to projects| pre| Assign(proj,dev) // try | unstrated | ? | unstarted |
| Unassign devlopers from projects| pre | Unassign(proj,dev) //try | unstrated | ? | unstarted |
| View assigned developers for current project| pre | GetDevelopers(...)| unstrated | view-develoeprs | unstarted |
| Search developer| unstrated | Get\<T>(...keys)| done | search-component | unstarted |
| Search project| unstrated | Get\<T>(...keys)| done | search-component | unstarted |


