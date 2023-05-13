// 1
//Task task = new Task(ActionTask);
//task.Start();

// 2
//Task task = Task.Factory.StartNew(ActionTask);

// 3
//Task task = new Task(ActionTask);


/*
Task taskOuter = new Task(() =>
{
    Task taskInner = Task.Factory.StartNew(() => {
        for (int i = 0; i < 100; i++)
            Console.WriteLine($"\t\ttask inner {i}");
    }, TaskCreationOptions.AttachedToParent);

    for (int i = 0; i < 50; i++)
        Console.WriteLine($"\ttask outer {i}");

});
taskOuter.Start();

for (int i = 0; i < 100; i++)
    Console.WriteLine($"main {i}");

*/
//task.Wait();
//task2.Wait();


// array of task
/*
Task[] tasks = new Task[3]
{
    new Task(() =>
    {
        for(int i = 0; i < 100; i++)
            Console.WriteLine($"\ttask 1 {i}");
    }),
    new Task(() =>
    {
        for(int i = 0; i < 100; i++)
            Console.WriteLine($"\t\ttask 2 {i}");
    }),
    new Task(() =>
    {
        for(int i = 0; i < 100; i++)
            Console.WriteLine($"\t\t\ttask 3 {i}");
    }),
};

foreach(var task in tasks)
    task.Start();

for (int i = 0; i < 50; i++)
    Console.WriteLine($"main {i}");

Task.WaitAny(tasks);
*/

// Task with Result
/*
int num = 1000;
Task<int> amountTask = new Task<int>((num) =>
{
    int amount = 0;
    for (int i = 1; i <= (int)num; i++)
    {
        amount += i;
    }
    return amount;
}, num);
amountTask.Start();


for (int i = 0; i < 10; i++)
    Console.WriteLine($"main {i}");

Console.WriteLine(amountTask.Result);
*/

/* Task Continue
Task taskFirst = new Task(() =>
{
    for(int i = 0; i < 10; i++)
        Console.WriteLine($"Task first {Task.CurrentId} {i}");
});

Task taskSecond = taskFirst.ContinueWith((task) =>
{
    Console.WriteLine($"\tprev task {task.Id}");
    for (int i = 0; i < 10; i++)
        Console.WriteLine($"\tTask second {Task.CurrentId} {i}");
});

taskFirst.Start();

taskSecond.Wait();
*/

// Task<TResult> Continue
/*
Task<int> amountTask = new Task<int>(() =>
{
    int a = 0;
    for (int i = 1; i <= 125; i++)
        a += i;
    return a;
});

Task<int> amountDigitsTask = amountTask.ContinueWith(t =>
{
    Console.WriteLine(t.Result);
    int s = 0;
    int num = t.Result;
    while(num != 0)
    {
        s += num % 10;
        num /= 10;
    }
    return s;
});

Task printTask = amountDigitsTask.ContinueWith(t =>
{
    Console.WriteLine(t.Result);
});
amountTask.Start();
printTask.Wait();
*/


// Parallel Invoke
/*
Action[] actions = new Action[3]
{
    () => { 
        for(int i = 0; i < 100; i++)
            Console.WriteLine($"Action 1 {i}");
    },
    () => {
        for(int i = 0; i < 100; i++)
            Console.WriteLine($"\tAction 2 {i}");
    },
    () => {
        for(int i = 0; i < 100; i++)
            Console.WriteLine($"\t\tAction 3 {i}");
    },
};

Parallel.Invoke(actions);
*/
//Parallel Loops
/*
Parallel.For(1, 10, (num, pls) => {
    if(num == 5) 
        pls.Break();
    Console.WriteLine($"{num}^2 = {num * num}"); 
});
Console.WriteLine();

Parallel.For(1, 10, ParallelAction);
Console.WriteLine();

List<int> list = new List<int>() { 10, 20, 30, 40, 50, 60, 70, 80, 90 };
Parallel.ForEach(list, (num) =>
{
    Console.WriteLine($"{num}^2 = {num * num}");
});
*/

// Cancellation
// Cancellation with return
/*
using (CancellationTokenSource cancellationTokenSource = new())
{
    CancellationToken cancellationToken = cancellationTokenSource.Token;

    Task task = new(() =>
    {
        for (int i = 0; i < 10; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"Task Cancel");
                return;
            }
            Console.WriteLine($"Task work {i}");
            Thread.Sleep(500);
        }
    }, cancellationToken);
    task.Start();

    Thread.Sleep(2000);
    cancellationTokenSource.Cancel();
    Thread.Sleep(1000);
    Console.WriteLine($"Task status {task.Status}");
}
*/

/*
using (CancellationTokenSource cancellationTokenSource = new())
{
    CancellationToken cancellationToken = cancellationTokenSource.Token;

    Task task = new(() =>
    {
        int i = 0;
        cancellationToken.Register(() =>
        {
            Console.WriteLine($"token is cancel message");
            i = 20;
        });
        for (i = 0; i < 10; i++)
        {
            //if (cancellationToken.IsCancellationRequested)
            //    cancellationToken.ThrowIfCancellationRequested();

            Console.WriteLine($"Task work {i}");
            Thread.Sleep(500);
        }
    }, cancellationToken);

    try
    {
        task.Start();

        Thread.Sleep(2000);
        cancellationTokenSource.Cancel();
        task.Wait();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    Thread.Sleep(1000);
    Console.WriteLine($"Task status {task.Status}");

}

*/
using (CancellationTokenSource cancellationTokenSource = new())
{
    CancellationToken cancellationToken = cancellationTokenSource.Token;
    Task task = new Task(() => PrintNumbers(cancellationToken), cancellationToken);
    
    try
    {
        task.Start();
        Thread.Sleep(2000);
        cancellationTokenSource.Cancel();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
    Thread.Sleep(1000);
    Console.WriteLine($"Task status {task.Status}");
}

void PrintNumbers(CancellationToken token)
{
    for(int i = 0; i < 10; i++)
    {
        if (token.IsCancellationRequested)
            token.ThrowIfCancellationRequested();

        Console.WriteLine($"task work {i}");
        Thread.Sleep(500);
    }
}


void ParallelAction(int num, ParallelLoopState pls)
{
    if (num == 5)
        pls.Break();
    Console.WriteLine($"{num}^2 = {num * num}");
}
void ActionTask()
{
    for(int i = 0; i < 100; i++)
        Console.WriteLine($"\ttask {i}");
}

void PrintTask(Task task)
{
    Console.WriteLine($"\tprev task {task.Id}");
    for (int i = 0; i < 10; i++)
        Console.WriteLine($"\tTask second {Task.CurrentId} {i}");
}