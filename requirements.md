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
| Create Project | done | Add\<T>(T x)| done | create-project-| done |
| Create Developer| done | Add\<T>(T x)| done | create-developer- | done |
| View Project| bata | Get\<T>(...)| done | project- | done |
| View Developer| beta | Get\<T>(...)| done| developer- | done |
| View Active Projects| done | GetActiveProjects(...)| done | active-projects  | done|
| View all developers| done | Get\<T>(...)| done |all-developers,list-developers| done |
| View all project| done | Get\<T>(...)| done | all-projects| done |
| Edit Project| done | Update\<T>(T x)| done | create-project | inherit |
| Edit Developer| done| Update\<T>(T x)| done | create-developer | inherit |
| View Completed | pre | GetProjects(...)| unstrated | all-projects | unstarted |
| View upcomming projects|  done | GetProjectsByStatus(...)| done| upcomming-projects | done |
| Delete projects/developers| skip? | Remove\<T>(T x)| done |  - | skip |
| Assign Developers to projects| done | Assign(proj,dev) | done | assign-button,assign-service | done |
| Unassign devlopers from projects| done | Unassign(proj,dev) | done | assign-button ,assign-service | done |
| View assigned developers for current project| done | GetDevelopers(...)| done | view-develoeprs | done |
| Search developer| done | Get\<T>(...)| done | list-developers | done|
| Search project| done | Get\<T>(...)| done | list-projects | done|


