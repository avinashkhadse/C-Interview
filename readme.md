The application logs login and logout events using three types of loggers:

FileLogger - Logs messages to a file (log.txt) in the solution directory.
ConsoleLogger - Logs messages to the console.
CompositeLogger - Uses both FileLogger and ConsoleLogger to log messages simultaneously.

Features
Allows any user to log in with a User ID and password.
Stores new users dynamically during runtime.
Provides an option to stay logged in or log out.
Logs login and logout activities along with timestamps and User ID.
Stores logs in a file located in the solution directory.
