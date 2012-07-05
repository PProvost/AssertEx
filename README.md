# AssertEx 

AssertEx is a simple library of helper methods for unit testing.
Originally created for use with MS-Test, it is framework neutral so should be
usable with any .NET unit testing framework.

# API

## AssertEx 

The static class that contains the helper methods

### Standard Exception Helpers

* `AssertEx.DoesNotThrow(delegate testCode)`
* `AssertEx.Throws(Type type, delegate testCode)`
* `AssertEx.Throws<T>(delegate testCode)`

### Task-Async Exception Helpers

* `AssertEx.TaskDoesNotThrow(delegate testCode)`
* `AssertEx.TaskDoesNotThrow<T>(delegate testCode)`
* `AssertEx.TaskThrows(Type type, delegate testCode)`
* `AssertEx.TaskThrows<T>(delegate testCode)`

## AssertExException 

An exception class that is used by AssertEx when an assertion fails. Nothing
fancy here.

# Credits

Many thanks to Jim Newkirk and Brad Wilson for the whole `Assert.Throws()` thing
in Xunit.net, from which this work is obviously and blatantly derived. And to shiftkey
for the Task-Async versions.