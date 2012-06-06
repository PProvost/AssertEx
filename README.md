# AssertEx 

AssertEx is a simple class of helper methods for unit testing.
Originally created for use with MS-Test, it is framework neutral so should be
usable with any .NET unit testing framework.

# API

## AssertEx 

The static class that contains the helper methods

### Methods

* `Assert.DoesNotThrow(delegate testCode)`
* `Assert.Throws(Type type, delegate testCode)`
* `Assert.Throws<T>(delegate testCode)`

## AssertExException 

An exception class that is used by AssertEx when an assertion fails.
