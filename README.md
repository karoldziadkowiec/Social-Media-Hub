# SocialMediaHub
## About project
The project is a REST controller for a social networking platform. It enables users of the social media platform to work effectively with data. All repository methods have been tested in the project by xUnit. 

## Technologies
- Framework: **ASP.NET Core Web API**
  - REST API
- Backend: **C#**
  - Multithreading (async/await)
  - LINQ Queries
  - Repository pattern
  - ClosedXML library
- ORM: **Entity Framework Core**
  - Migrations
- Database: **Microsoft SQL Server**
- Unit tests: **xUnit**
- API testing:
  - **Swagger UI**
  - **Postman**

## Images
Entities and database tables created:

![1](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/1.png)
![2](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/2.png)

Entity endpoints created:
![3](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/3.png)
![4](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/4.png)
![5](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/5.png)
![6](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/6.png)
![7](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/7.png)
![8](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/8.png)
![9](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/9.png)

Example endpoints tested by Swagger UI:
- POST: /api/users -> adding new user

![10](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/10.png)

- GET: /api/users -> displaying users
  
![11](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/11.png)

- GET: /api/users/csv -> displaying users in CSV file using ClosedXML library
  
![11.2](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/11.2.png)
![11.3](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/11.3.png)

- GET: /api/users/:id -> displaying entered user

![12](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/12.png)

- PUT: /api/users/:id -> editing entered user

![13](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/13.png)
![14](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/14.png)

- DELETE: /api/users/:id -> removing entered user

![15](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/15.png)

- POST: /api/friendships -> adding friendship between users: id=2 and id=3

![16](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/16.png)

- GET: /api/groups/:id/fill -> calculating group filling in %

![17](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/17.png)

- GET: /api/groups/search -> searching grups by entered group name

![18](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/18.png)

- POST: /api/posts/:postId/comments -> adding comment to the existing post

![19](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/19.png)

- POST: /api/posts/:postId/likes -> like existing post

![20](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/20.png)

Unit testing all repository methods with xUnit:

![21](https://github.com/karoldziadkowiec/Social-Media-Hub/blob/master/photos/21.png)
