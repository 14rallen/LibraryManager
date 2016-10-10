echo off

rem batch file to run a script to create a db
rem 9/4/2014

rem uses the sqlcmd utility to run the sql script

rem -S = server
rem -E = trusted connection
rem -i = input file

rem replace the sql script name with your real script name
rem also, if you just took the defaults when install sqlexpress,
rem your sql server instance is probably localhost\sqlexpress
rem example: sqlcmd -S localhost\sqlexpress -E -i dunebuggies.sql
sqlcmd -S localhost -E -i library.sql

ECHO if no error messages appear DB was created
PAUSE