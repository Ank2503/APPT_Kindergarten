# Type Guesser

The result of this task should be done in a form of console application. 

It is preferable to use .NET Core console application instead of .NET full framework.

Application should accept only 1 command line argument. If number of arguments is higher than 1 or equals zero - error has to be shown to the user in console and application should terminate after Enter key press. 

This one and only argument is a number which should be validated for fitting following types range:

* byte
* sbyte
* short (both signed and ushort)
* int (both signed and uint)
* long (both signed and ulong)
* float 
* double

For example, if I start "HomeWork1.exe 890" I expect to get following output:

1. byte - false (over limit = 634)
2. sbyte - false (over limit = 762)
3. int - true
4. uint - true
5. long - true
6. ulong - true
7. float - true
8. double - true

So you need to numerate the list with those types, provide true or false that indicates if the provided number can fit into the type value range and in brackets calculate over limit (or under limit if we speak about negative numbers, please also implement that check) and output everything to console.

Application should wait for Enter key press to terminate. 

In case input can not be parsed (is not a number at all) - show error text to the user in console and terminate after Enter key press.



## Things to mind

1. Code style.
2. Exceptions handling.
3. Try to keep code as short as possible without loosing readability. No code duplication please!
4. Every developer is expected to work in his branch called under his login in email (d.severin, a.budnik).
5. On completion of task, pull request should be sent to the  master branch https://docs.microsoft.com/en-us/vsts/git/pull-requests
6. Mind the code uniqueness. I find no way to make your branches private, therefore result of your work will be visible to everyone. If I find out that anyone copied someone else code (I will use my personal judgements, no arguments will be accepted), person with last commit gets a warning. Two warnings means you are out of development training. 

If you have any questions regarding this task, please let me know in Slack chat by 13:00 tomorrow (19/01/2018).

Thank you and let's get it started!
