# ModelPlaneInfo.ConsoleEditor

Old project with minimal changes made due to migration of the project from the .NET Framework 4.7.2. platform to the .NET Core 6.0 platform. The project was developed as a final project from the "Programming" educational subject to confirm its successful completion

## Project structure
```bash
ModelPlaneInfo.ConsoleEditor:
¦   ModelPlaneInfo.ConsoleEditor.sln
¦   
+---Common
¦   ¦   Common.csproj
¦   ¦   
¦   +---ConsoleIO
¦   ¦       Entering.cs
¦   ¦       Settings.cs
¦   ¦       
¦   +---Entities
¦   ¦       OutString.cs
¦   ¦       
¦   +---Extensions
¦   ¦       EnumerableMethods.cs
¦   ¦       
¦   L---Interfaces
¦           IEntity.cs
¦           IKeyable.cs
¦           ITestStorage.cs
¦           
+---Common.Context
¦   ¦   Common.Context.csproj
¦   ¦   
¦   +---Extensions
¦   ¦       EnumerableMethods.cs
¦   ¦       StringMethods.cs
¦   ¦       
¦   +---LineIndents
¦   ¦       LineIndent.cs
¦   ¦       SimpleLineIndent.cs
¦   ¦       
¦   L---StringFormatters
¦           CorrectStringFormatter.cs
¦           SimpleStringFormatter.cs
¦           StringFormatter.cs
¦           
+---Common.Repositories
¦   ¦   Common.Repositories.csproj
¦   ¦   Repository.cs
¦   ¦   UnitOfWork.cs
¦   ¦   
¦   L---Interfaces
¦           IRepository.cs
¦           IUnitOfWork.cs
¦           
+---ModelPlaneInfo
¦   ¦   ModelPlaneInfo.csproj
¦   ¦   
¦   +---Data
¦   ¦       DataContext.cs
¦   ¦       DataContext.TestingData.cs
¦   ¦       
¦   +---Entities
¦   ¦       ModelPlane.cs
¦   ¦       ModelPlane.validation.cs
¦   ¦       PlaneType.cs
¦   ¦       PlaneType.validation.cs
¦   ¦       
¦   +---Interfaces
¦   ¦       IDataSet.cs
¦   ¦       IFileIoController.cs
¦   ¦       
¦   L---IO
¦           BinarySerializationController.cs
¦           GenericBinarySerializationController.cs
¦           TextFileIoController.cs
¦           XmlFileIoController.cs
¦           
+---ModelPlaneInfo.ConsoleEditor
¦   ¦   ModelPlaneInfo.ConsoleEditor.csproj
¦   ¦   Program.cs
¦   ¦   
¦   +---Commands
¦   ¦       CommandInfo.cs
¦   ¦       CommandManager.cs
¦   ¦       MainManager.cs
¦   ¦       
¦   +---Controllers
¦   ¦       ModelPlaneFiltrationController.cs
¦   ¦       PlaneTypeFiltrationController.cs
¦   ¦       
¦   L---Editors
¦           ModelsPlaneEditor.cs
¦           PlaneTypeEditor.cs
¦           
L---ModelPlaneInfo.Repositories
    ¦   FileBasedUnitOfWork.cs
    ¦   ModelPlaneInfo.Repositories.csproj
    ¦   
    L---Interfaces
            IInfoUnitOfWork.cs
```

- **`Common`** - contains auxiliary classes;
- **`Common.Context`** - contains classes for working with text;
- **`Common.Repositories`** - contains classes to implement helper classes for the Repository and Unit of Work patterns;
- **`ModelPlaneInfo`** - contains classes for working with data;
- **`ModelPlaneInfo.Repositories`** - contains classes for implementing the Repository and Unit of Work patterns;
- **`ModelPlaneInfo.ConsoleEditor`** - contains classes for implementing data manipulation tools.

## Patterns

The landing gear project includes the following patterns:
- `Strategy`
- `Command pattern`
- `Unit of Work`
- `Repository`

<img src="https://user-images.githubusercontent.com/70714177/219955428-c7509f6c-62f6-47e4-849d-dd9999d0f16a.png" width="50%"></img>

**Image 1 - The structure of classes for the implementation of the controller for saving data to a file (Strategy pattern)** 


The Strategy pattern allows you to define a family of interchangeable algorithms and make them interchangeable at runtime. In this case, the Save and Load methods are dependent on a fileIoController object that implements the IFileIoController interface. By using the IFileIoController interface, the classes can work with different types of file I/O controllers without needing to know their specific implementations. This allows for greater flexibility in the application and makes it easier to swap out different file I/O controllers at runtime. Overall, the Save and Load methods act as the context in the Strategy pattern, while the IFileIoController interface and its various implementations represent the strategy.

<img src="https://user-images.githubusercontent.com/70714177/219955930-aab39b8e-cd6d-4c76-89ac-4a3a6bd763eb.png" width="50%"></img>

**Image 2 - Class structure for the command manager implementation(Command pattern)**

The `Command` delegate represents an operation to be executed. The `CommandInfo` struct acts as a container for a command and its name. This pattern allows you to encapsulate requests or operations as objects, which can be used to parameterize methods. This makes it possible to delay execution of the request, queue it for later execution, and support undoable operations.
In this case, the `Command` delegate represents the operation that needs to be executed, while the `CommandInfo` struct provides a way to associate a name with a command. This allows you to create a collection of `CommandInfo` objects, which can be used to represent a list of commands that can be executed.
Overall, the Command pattern provides a way to decouple the requester of an action from the object that performs the action, and allows for greater flexibility and extensibility in an application.

<img src="https://user-images.githubusercontent.com/70714177/219956981-11bd0961-b9a9-41e4-aee6-1093f1becaa8.png" width="50%"></img>

**Image 3 - Implementation of the Unit of work and Repository patterns**

The `Unit of Work` and `Repository` patterns are two widely used design patterns in software development, often used together in data access layers. 
The `Repository` pattern provides an abstraction layer between the application code and the database, providing a clean separation of concerns between data access and business logic. This pattern defines an interface for data operations, which is implemented by a concrete repository class that performs database operations. By doing so, it allows developers to create a single, consistent data access layer for the application.
The `Unit of Work` pattern manages the transactions and the connectivity to the data source. It provides a way to group multiple database operations into a single transaction, ensuring that all database changes are consistent and atomic. The Unit of Work pattern also ensures that multiple repository objects can share a single database connection, reducing the number of connections used and improving performance.
Together, these patterns provide a way to create a robust and scalable data access layer. The Repository pattern abstracts away the details of the database, while the Unit of Work pattern manages the transactions and connections. This separation of concerns makes the code easier to maintain and test, and makes it easier to swap out the underlying database without affecting the rest of the application.

## Conclusions

This project is my first program developed in C#. It contains many interesting solutions, but also a lot of mistakes. The implementation of the project is quite available and direct, but not bad. In the process of migration from the .NET Framework 4.7.2 platform. to the .NET Core 6.0 platform, the source code was almost unchanged. Corrections were made to the source code of some modules to fix some bugs.

## License

Distributed under the MIT License. See `License.txt` for more information.

