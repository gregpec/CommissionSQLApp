# CommissionSQLApp

Commission App is a console application for managing customer and car data in an SQL database. 
Users can perform various operations on the data, such as adding, displaying, deleting, sorting, 
exporting to files, and importing from files. Below is a detailed description of the available functionalities
Main Menu.When the application starts, the main menu is displayed with the following options:

Adding Listing Data:
    1.Add a customer to the SQL database
    (Allows adding a new customer whose data will be saved in the database.)
    2.Add a car to the SQL database
    (Enables adding a new car to the database.)
    3.Display all customers from the SQL database
    (Displays a complete list of customers stored in the database.)
    4.Display all cars from the SQL database
    (Shows a list of all cars available in the database.)
    5.Display all customers and cars from the SQL database
    (Combines customer and car data, displaying them in a single list.)

Deleting Data:
    6.Delete all customers from the SQL database
    (Removes all customers from the database.)
    7.Delete all cars from the SQL database
    (Removes all cars from the database.)
    8.Delete all customers and cars from the SQL database
    (Deletes all data for both customers and cars.)
    9.Delete a customer by ID
    (Deletes a specific customer based on their unique identifier.)
    10.Delete a car by ID
    (Deletes a specific car based on its unique identifier.)

Auditing and Data Import/Export:
    11.Display the audit file
    (Shows logged information about operations performed in the application.)
    12.Import car data from the Cars.csv file
    (Imports car data from a CSV file into the database.)
    13.Import customer data from the Customers.csv file
    (Imports customer data from a CSV file into the database.)
    14.Create JSON files for customers and cars
    (Exports data from CSV files (Customers.csv and Cars.csv) to JSON files.)
    15.Load data from JSON files into SQL
    (Imports customer and car data from JSON files into the database.)
    16.Create an XML file for cars
    (Exports data from the Cars.csv file to an XML file.)

Data Operations
    17.Sort car data from SQL by price
    (Sorts cars in the database by price in ascending order.)
    18.Display cars more expensive than 500,000 from SQL
    (Filters and displays cars priced above 500,000.)
    19.Display customers who can buy cars for a given price
    (Lists customers who can afford cars at the specified price.)
    20.Cars a customer can buy based on their budget
    (Displays a list of cars that each customer can afford based on their maximum budget.)
    21.Export cars grouped by customers to an XML file
    (Exports car data grouped by customers who can afford them and saves the result in a CarsByCustomers.xml file)

Summary:
The Commission App provides a comprehensive toolkit for managing customer and car data. With support for CSV,
JSON, and XML file formats, integration with other systems is convenient and flexible. The SQL database ensures
efficient storage and manipulation of large datasets. This application is ideal for businesses that need 
a streamlined way to manage sales-related data and analytics.