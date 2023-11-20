# Asynchronous Programming

To learn about async programming review `Program.cs`, a simple console application that shows different implementations of something that does the same: wait and print. Here is a description on the various implementations:

## Various implementations

- **Step1**: this is a synchronous function because it is a void: when it returns, everything inside the function has been executed. If there is an async call (like in Step4), then it might finish or it might not.
- **Step2**: `Task.Delay` returns a task immediately. You can use that task to check if it is finished. You need to return the task to check if it is done or not. `Task.CompletedTask` is the state of that task: if you return `Task.CompletedTask` it will give you that it is complete. So this is ***bad*** code because you're saying the task is completed but it might not be.  
- **Step3**: this is a synchronous version to wait; the thread that calls this now will also be blocked. The implementation is technically correct, but not efficient.
- **Step4**: this is the correct way of doing. `await` on a `Task` means that the print statement inside the Step4 function (line 40) is not run until the await is finished. So when calling Step4, it will return immediately, but will run line 40 only once its complete. When calling `Step4()` you should always use `await Step4()`: await and async always go together and as it returns a `Task` (promise to eventually finish) this also goes with it. 

This is what the output looks like:
```csharp
Step 1 - 11/20/2023 12:29:32 PM
Inside Step 1 - 11/20/2023 12:29:34 PM
Step 2 - 11/20/2023 12:29:34 PM
Inside Step 2 - 11/20/2023 12:29:34 PM
Step 3 - 11/20/2023 12:29:34 PM
Inside Step 3 - 11/20/2023 12:29:34 PM
Step 4 - 11/20/2023 12:29:36 PM
Inside Step 4 - 11/20/2023 12:29:38 PM
```


## Way of writing

The KEY here is that you can WRITE the code as if it is synchronous, look at this difference:

```csharp
static int GetNumber(){
    return 4;
}
static async Task<int> GetNumberAsync(){
    await Task.Delay(2000);
    return 3;
} 
```
`GetNumberAsync` is the async version of `GetNumber` including a 2 second wait. Now If you call:
```csharp
var t = GetNumberAsync()
``` 
`t` is now of type `Task<int>`, representing the ongoing operation and not yet the integer result. You can access the integer result via:
```csharp
var number1 = await t;
```
this unwraps the task and retrieves the result once the task is complete. 
```csharp
var number2 = await GetNumberAsync(); //you will get the int directly.
var number3 = GetNumber(); //hides the async complexity. 
```
The use of `await`` makes the code look synchronous. It allows the rest of the method to be written as if it were a straight-line code, which is easier to read and maintain.
The async-await pattern handles the complexities of asynchronous programming (like continuations and callbacks) behind the scenes.

## Cancellation token

An async run is something you might want to cancel. This can be done by implementing the CancellationToken.

```csharp
static async Task<int> GetNumberAsync(CancellationToken cancellationToken){
    await Task.Delay(2000);
    return 3;
} 
var t = GetNumberAsync(CancellationToken.None);
```
Passing on `CancellationToken.None` means the run can't be cancelled: this is useful for unit tests.

## Assignment

Play around with this, write some of your own very simple code to get used to it. This should become second nature. 
In Visual Studio, under View, there are some useful tools:
- Task list: shows all the active tasks. You need to be in debug mode in order to see it. Hit Debug >> Windows >> Tasks.
- Threads is where you can see the threads. 
- Call Stack is where you can see what is called in which order.
