# Domain-Driven Design and MVVM Implementation Guides

This repository contains implementation guides and best practices for Domain-Driven Design (DDD) and Model-View-ViewModel (MVVM) patterns in .NET and TypeScript/React applications.

## Table of Contents

- [Introduction](#introduction)
- [Domain-Driven Design](#domain-driven-design)
  - [Event-Domain-Driven Design in .NET Loan Management](#event-domain-driven-design-in-net-loan-management)
  - [Domain-Driven Design in .NET with Loan Management](#domain-driven-design-in-net-with-loan-management)
- [Model-View-ViewModel (MVVM)](#model-view-viewmodel-mvvm)
  - [MVVM with TypeScript and React](#mvvm-with-typescript-and-react)
  - [Parent ViewModels in MVVM with TypeScript and React](#parent-viewmodels-in-mvvm-with-typescript-and-react)
- [Contributing](#contributing)
- [License](#license)

## Introduction

This repository serves as a comprehensive guide for implementing Domain-Driven Design (DDD) and Model-View-ViewModel (MVVM) patterns in modern software development. The examples and patterns demonstrated here are based on real-world applications, particularly focusing on loan management systems and modern web applications.

## Domain-Driven Design

Domain-Driven Design is an approach to software development that focuses on understanding the business domain and creating a software model that reflects it.

### Event-Domain-Driven Design in .NET Loan Management

This section explains how to implement Event-Domain-Driven Design in a .NET loan management system. It demonstrates how to create a maintainable and scalable solution that accurately reflects business domain concepts.

Key topics include:
- Implementing aggregates and entities
- Working with value objects
- Creating a rich domain model with behavior
- Implementing domain events
- Validation as part of the domain
- Command handlers in the application layer

[Read the full article](https://override.dev/implementing-event-domain-driven-design-in-a-net-loan-management-system)

### Domain-Driven Design in .NET with Loan Management

This section demonstrates a practical implementation of Domain-Driven Design in a .NET loan management system. It focuses on creating a maintainable and scalable architecture that accurately represents business rules.

Key topics include:
- Creating aggregates and entities
- Implementing value objects
- Building a domain model with behavior
- Working with domain events
- Encapsulating business rules and validations
- Using the Result pattern for error handling

[Read the full article](https://override.dev/implementing-domain-driven-design-in-net-with-loan-management)

## Model-View-ViewModel (MVVM)

MVVM is a design pattern that separates the development of the graphical user interface from the business logic or back-end logic of the application.

### MVVM with TypeScript and React

This section explains how to implement the MVVM pattern using TypeScript and React, bringing desktop development patterns to web applications. The implementation focuses on a loan management application with complex forms.

Key topics include:
- Creating reactive models with RxJs
- Building ViewModels for form validation
- Using the Specification pattern for validation
- Connecting React views with ViewModels
- Creating a clear separation of concerns

[Read the full article](https://override.dev/mvvm-with-typescript-and-react-for-net-developers)

### Parent ViewModels in MVVM with TypeScript and React

This section builds on the basic MVVM implementation, focusing on Parent ViewModels that orchestrate multiple child ViewModels in a Stepper component for multi-step interfaces.

Key topics include:
- Creating a centralized registry for ViewModels
- Building a Parent ViewModel to manage child ViewModels
- Standardizing ViewModel interfaces
- Implementing reactive Parent-Child communication
- Managing validation across multiple forms

[Read the full article](https://override.dev/mastering-mvvm-with-typescript-and-react-part-2-parent-viewmodels)
