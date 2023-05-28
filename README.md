# Politicz.News

Politicz.News is a reference project that demonstrates the usage of Mediator, Entity Framework Core, Minimal API, and Docker in an ASP.NET Core back-end application. The project aims to provide a reliable source of news and information on politics.

## Features

- CRUD operations for news

## Technologies Used

- ASP.NET Core 7.0
- Entity Framework Core
- Mediator pattern
- Microsoft SQL Server
- Minimal API
- Docker

## Environment variables
```text
- NewsApi_Database__ConnectionString
- NewsApi_JwtSettings__Authority
- NewsApi_JwtSettings__Audience
```

## Getting Started

To get started with Politicz.News, follow these steps:

1. Clone the repository: `git clone https://github.com/yourusername/Politicz.News.git`
2. Build the Docker image: `docker build -t politicznews .`
3. Run the Docker container with all necessary env variables: `docker run -p 8080:80 politicznews`

## Contributing

Contributions to Politicz.News are welcome and encouraged! To contribute, follow these steps:

1. Fork the repository
2. Create a new branch for your changes: `git checkout -b my-feature-branch`
3. Make your changes and commit them: `git commit -am 'Add some feature'`
4. Push your changes to your fork: `git push origin my-feature-branch`
5. Submit a pull request to the main repository

## Contact

If you have any questions or suggestions, please feel free to contact us. We'd love to hear from you!
