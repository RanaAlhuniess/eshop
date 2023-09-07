# Product management system - eshop

This repository contains a sample project demonstrating the implementation of a product management system using the ABP (ASP.NET Boilerplate) framework. The goal of this project is to showcase the skills and abilities required for building a robust and maintainable product management system with the following features:
## Features:
* Implementing CR** operations for Products and thier attributes and variants.
* CRUD operations fo Attributes and thier variants.
* Multilingual product name and description support.
* Handling product images, including the ability to set a main image for a product and assign images to product variants.
* Implementing a GetList endpoint with filtering, sorting, and paging capabilities.

-----------

# Getting started
Please check the official ABP Framework installation guide for server requirements before you start. [Official Documentation](https://docs.abp.io/)
Follow these steps to set up and run the project locally:
## Prerequisites:
* .NET Core SDK 7.0+
* Visual Studio or Visual Studio Code (optional, but recommended)
* SQL Server.
* Node v16.x

## Installation

- Clone the repository
```
    git clone  <git hub template url> <project_name>
```
- Switch to the repo folder
```
    cd <project_name>
```

## How to run

The application needs to connect to a database. Run the following command in the `eshop` directory:

````bash
dotnet run --migrate-database
````
This will create and seed the initial database. Then you can run the application with any IDE that supports .NET.

Start the local development server
````bash
dotnet ef run
````
You can now access the server at https://localhost:44318/
If you open Swagger UI by entering the /swagger URL in your application, you can see the eshop APIs:


# API Endpoints
The following endpoints are available for managing products and their attributes/variants:
## Product
GET /api/products: Get a list of products with filtering, sorting, and paging options.
GET /api/products/{id}: Get details of a specific product by ID.
POST /api/products: Create a new product.
POST /api/products/{id}/product-variants: Create a new attributes and variants of a specific product by ID.
DELETE /api/products/{id}: Delete a product by ID.

## Attributie
GET /api/app/product-attribute : Get a list of attributes with thier variants, and  with filtering, sorting, and paging options.
GET /api/product-attribute/{id}: Get details of a specific attribute by ID.
POST /api/product-attribute: Create a new attribute with variants.
PUT /api/product-attribute/{id}: Update an existing attribute by ID.
PUT /api/product-attribute/{id}/attribute-And-variants: Update an existing attribute by ID and update its variants.
DELETE /product-attribute/{id}: Delete a product by ID.

Happy coding..!



