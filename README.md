## Console Project – Shapes, Calculator & RPS

This project is a C#/.NET console application where you can calculate areas for various geometric shapes, use a mini calculator, and play Rock-Paper-Scissors against the computer. The project is built according to modern principles and methods, and demonstrates good architecture and code structure. It was developed as part of a course assignment in systems development.

## Table of Contents
- [Project Description](#project-description)
- [Technologies and Architecture](#technologies-and-architecture)
- [Design and UX](#design-and-ux)
- [Methods, Patterns and Principles](#methods-patterns-and-principles)
- [Key Features](#key-features)
- [Diary](#diary)
- [Trello Board](#trello-board)
- [Installation](#installation)

## Project Description

This application offers three main functions:

Shapes: Calculate area and perimeter for rectangle, triangle, parallelogram and rhombus.

Calculator: Perform basic mathematical calculations (addition, subtraction, multiplication, division, modulus, square root).

Rock-Paper-Scissors (RPS): Play Rock-Paper-Scissors against the computer. Statistics and history are saved.

The purpose is to practice:

Architecture according to layered principles (separation of presentation, logic and data)

Using design patterns and professional code structure

Database usage and best practices

## Technologies and Architecture

C# / .NET 9 – Foundation for the application

Entity Framework Core – For database access (Code First)

SQL Server – Database engine

Autofac – Dependency Injection (IoC container)

Spectre.Console – For better console experience (menus, tables, color)

GitHub – Version control, feature branches and descriptive commit messages

The project is divided into several layers:

Presentation: Handles user interface and menus

Logic: Handles all logic

Data: Entity Framework Core, DbContext and migrations

## Design and UX

The console interface uses Spectre.Console to provide a more professional feel.

Clear and easy-to-navigate main menu, logical flow between parts.

Presentation of results and statistics in table format with color and clarity.

Input validation to avoid incorrect input.

## Methods, Patterns and Principles

The project is based on several professional methods and design principles:

Object-Oriented Programming (OOP) – Small, focused classes/methods.

Layered Architecture – Clear separation of responsibilities.

Dependency Injection (DI) – Loose coupling, easy to replace components, enables testability.

Strategy Pattern – For the calculator's different calculations.

ViewModels – Clear transport of data between layers.

DRY Principle – No code duplication.

Separation of Concerns – Presentation, logic and data are separated.

ModelState/Validation – Prevents incorrect input.

## Key Features

CRUD on shapes/calculator history/rps game rounds (Create, Read, Delete)

Automatic calculation of area and perimeter for different shapes

Different strategies for the calculator (for example square root for only one number)

Save and display history/statistics for RPS matches

Display leaderboards/statistics on matches (RPS)

Input validation everywhere

User-friendly, clear and robust console interface

## Diary

In a Diary.md file I have documented my work on this project. It can also be found in this repository.

## Trello Board

Link to my Trello board for work tracking:

https://trello.com/invite/b/681a6b9eb373b37621f5afc9/ATTI6116934026d48aa2bdbafb5880971a7cF824F954/former-miniraknare-och-sten-sax-och-pase-console-project

## Installation

Clone this repository via this link: https://github.com/maxiimize/Console-Project-Shapes-Calculator-RPS.git

Open the project in Visual Studio

Build the project
