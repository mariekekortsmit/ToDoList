# ToDoList

Repository for learning C# by working through a ToDoList application in C#.

## To do list

To be filled during next coaching meeting.

## History of to do list and related notes

1. Replace all foreach loops with LINQ;
    - Question: For the Update and Get(id) functions, what to use? First? FirstOrDefault? SingleOrDefault? Where could give multiple entries so is less efficient as it will keep on looking after you've found one, and in our setup there should only be 1 item with this specific id.  
2. Replace the inmemory database with an interface;
3. Add a seperate implementation for the database that uses a dictionary instead of a list.
    - Question: Is the place where I decide on which one to use in Program.cs? That means if I want to use the other one I need to change the code. That doesn't seem ideal.
4. Split out classes and files in different folders.
    - Question: Folders are now called DataAccess (for the data interface), Models (for the data models), Services (for the database implementations). What is a typical naming?