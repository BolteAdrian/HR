# HR
Project:
CVs and interviews management
.NET Core 3.1
HTML.Model and HTML.Console App must be taken out of the main file for them to work.
In ConsumeModel.cs we need to change the path for  HRML.Model\MLModel.zip

We need to create the database from the model (entities and context) by adding a migration.

We can execute the migration command using NuGet Package Manger Console as well as dotnet CLI (command line interface).

In Visual Studio, open NuGet Package Manager Console from Tools -> NuGet Package Manager -> Package Manager Console and enter the following command:

PM> add-migration CreateSchoolDB


 How to use the application

1. System registration and authentication

 When accessing the home page, before registration, 3 buttons are displayed in the middle of the page (Figure 1) that lead to 3 sections of the application, but without the possibility of being used to go further in the application while we are not logged in, and registration or authentication options are provided in the navigation bar. If we are not registered we can only enter the page with the operation policy of the site.

![image](https://user-images.githubusercontent.com/87446991/165993954-8c24b562-bffc-413d-b17a-a7fd8d875b16.png)

• Registration - for registration, the following data is required: email and a password (Figure 2);
![image](https://user-images.githubusercontent.com/87446991/165994047-d9c6ed0f-5ab6-4a05-a105-fd2d4b850065.png)

• Authentication - authentication is based on email and password (Figure 3);
![image](https://user-images.githubusercontent.com/87446991/165994156-9b20fc65-c8b3-464c-8b0d-9fd2a9769594.png)

There are 2 types of account roles:

1. Admin
-can use all the functionalities of the application;
-only the person who manages the database can act as admin;


2. Observer
-any newly created account has the role of observer;
-they cannot insert, edit or delete anything from the tables, and if they try to be redirected to the page with the message "Access denied" (Figure 4);

2. CV management

Once the user logs in, they will have access to the 3 sections of the application. In the CV section, a page will open with a table in which all those who submitted their CV to the company are listed (Figure 5). The user will be able to search the table to sort the search bar or table according to the desired section (name, id, experience, etc.).

![image](https://user-images.githubusercontent.com/87446991/165995557-0fe6a76b-4e18-4d80-8c55-0338e229b066.png)

2.1 Data management

Admin users will be able to:
-Add new people to the table and import their resume into the application (Figure 6);
-Modify the existing data in the table (Figure 7);
-Delete data (Figure 8);

![image](https://user-images.githubusercontent.com/87446991/165995713-754e3080-839b-4573-a4e0-d57b5f015d4b.png)
![image](https://user-images.githubusercontent.com/87446991/165995729-7457c834-f2f1-45cd-a8a0-04e7ddcba489.png)

![image](https://user-images.githubusercontent.com/87446991/165995751-5339b65d-f34c-4a94-9f70-2a9245b3b293.png)
![image](https://user-images.githubusercontent.com/87446991/165995780-7c0dfc99-e099-4280-bd44-da0df7925fd2.png)

![image](https://user-images.githubusercontent.com/87446991/165995795-ea75d09c-c048-410b-b87c-72839e375706.png)


2.2 Details

In the details area to find information from a person's table, the interviews he participated in and the submitted CV (Figure 9);
![image](https://user-images.githubusercontent.com/87446991/165995919-5d0d2aa7-6865-4c00-8f01-f1654314749f.png)
![image](https://user-images.githubusercontent.com/87446991/165995930-cf58ad50-1a2d-44a7-a403-13cde10b3a13.png)


2.3 Printing and exporting

Users will be able to print data (Figure 10), export the data to an excell file (Figure 5.11) or send data via email (Figure 12);
![image](https://user-images.githubusercontent.com/87446991/165996220-b5dddc97-6efb-41ea-a6f2-5444a3186116.png)

![image](https://user-images.githubusercontent.com/87446991/165996229-1566d0e6-dbf4-4915-a64c-60527b3f7198.png)

![image](https://user-images.githubusercontent.com/87446991/165996244-b0c26d0b-f967-4a42-9c7a-1cc7d5d968f1.png)

2.4 Statistics and prediction

From the CV page you can access 2 sections "Statistics" and "Prediction" (Figure 13)
![image](https://user-images.githubusercontent.com/87446991/165996439-36bfd5c1-f1d9-4c01-9835-07adc11a5781.png)


2.4.1	Statistics

	În această pagină se poate vizualiza un grafic împărțit pe toate lunile anului cu numărul de cv-uri primite lunar (Figura 14). Acest grafic poate fi și printat sau vizualizat în mod full screen.
  
  ![image](https://user-images.githubusercontent.com/87446991/165996801-3f75b232-748e-4b7b-b13a-908d7fa12e2c.png)

2.4.2	Predicție

	În această pagină se poate vizualiza un tabel cu toate persoanele care și-au depus cv-ul și o predicție asupra șanselor lor de a fi angajați în funcție de 7 date de intrare(nume, dată nașterii, funcția la care aplică, departamentul, studii, experiență, mod de aplicare și observații)  ( Figura 15);
  
  ![image](https://user-images.githubusercontent.com/87446991/165996943-bb27618f-00e3-4d1c-9ebd-83e438c6fe6c.png)

3 Interview management

In the "Interview" section, a page will open with a table listing all the interviews conducted in the company (Figure 16). The user will be able to search the table using the search bar or sort the table according to the desired section (name, id, interview day, etc.).

![image](https://user-images.githubusercontent.com/87446991/165997733-da031b8e-86ec-4a6e-90f9-d4606b999ca2.png)

3.1 Data management

Admin users will be able to:
-Add new interviews to the table (Figure 17);
-Modify the existing data in the table (Figure 18);
-Delete data (Figure 19);

![image](https://user-images.githubusercontent.com/87446991/165997844-bab50351-4a32-4511-9f55-f6f9e30f970a.png)

![image](https://user-images.githubusercontent.com/87446991/165997858-722b1b50-c4d0-4dc1-8245-5c91519ad90e.png)

![image](https://user-images.githubusercontent.com/87446991/165997877-655dc1a9-2485-4ca2-b14c-72ea1775f5bb.png)

3.2 Printing and exporting

Users will be able to print data (Figure 20), export the data to an excell file (Figure 21) or send data via email (Figure 22);

![image](https://user-images.githubusercontent.com/87446991/165997956-c127b889-6ad5-4950-ae9a-8d9d115725f9.png)

![image](https://user-images.githubusercontent.com/87446991/165997973-ab5fe504-27b6-4330-a149-fb8692bf956e.png)

![image](https://user-images.githubusercontent.com/87446991/165997997-e277c67f-2562-4439-8383-c6f81897aa88.png)

3.3 Statistics

In the Statistics section you can see a graph divided by all months of the year with the number of people employed per month following the interview (Figure 23). This graphic can also be printed or viewed in full screen.

![image](https://user-images.githubusercontent.com/87446991/165998360-7d98d406-d8cf-49d9-a9ba-13ec5fd0c436.png)

3.4 Employees

In the "Employee" section, a page will open with a table listing all the employees involved in the interviews (Figure 24). The user will be able to search the table to sort the table according to the desired section (name, id, organization, email, etc.).

![image](https://user-images.githubusercontent.com/87446991/165998497-c3f82c0b-794f-4aff-ad1d-36c2d7fb9f29.png)

3.4.1 Data management

Admin users will be able to:
-Add new employees to the table (Figure 25);
-Modify the existing data in the table (Figure 26);
-Delete data (Figure 27);

![image](https://user-images.githubusercontent.com/87446991/165998806-3c790aef-0025-4421-8e40-6e5cf751ab85.png)

![image](https://user-images.githubusercontent.com/87446991/165998823-e7782b77-ec71-4b97-aa5c-179c90b86a7d.png)

![image](https://user-images.githubusercontent.com/87446991/165998842-3d2ca6fb-432a-44fb-ab77-60359616960a.png)

3.4.2 Printing and exporting

Users will be able to print data (Figure 5.28), export data to an excell file (Figure 29), or send data via e-mail (Figure 30);

![image](https://user-images.githubusercontent.com/87446991/165998945-dc50df93-81e8-406b-8300-6505abcbc784.png)

![image](https://user-images.githubusercontent.com/87446991/165998964-45c03202-ce76-45f5-b984-b8b101b075a8.png)

![image](https://user-images.githubusercontent.com/87446991/165998983-d019585f-a73f-438f-b194-39bd1b80c57e.png)

4 The Department

In the "Departments" section, a page will open with a table listing all the departments in the company (Figure 31). The user will be able to search the table to sort the search bar or the table according to the desired section (id and name of the department).


![image](https://user-images.githubusercontent.com/87446991/165999224-334ec1af-a996-4cc5-8d60-aeed7baf1f14.png)

4.1 Data management

Admin users will be able to:
-Add new departments to the table (Figure 32);
-Modify the existing data in the table (Figure 33);
-Delete data (Figure 34);

![image](https://user-images.githubusercontent.com/87446991/165999321-0fdc1d18-b0f8-40a0-aab5-7d85c54d6a55.png)

![image](https://user-images.githubusercontent.com/87446991/165999342-1fe189f2-e599-4af7-8bd5-4218a19fdc27.png)

![image](https://user-images.githubusercontent.com/87446991/165999361-d02937e1-8ea5-4757-9d7c-3ea9e81d582e.png)

4.2 Functions

In the "Functions" section, a page will open with a table listing all the functions in the company (Figure 35). The user will be able to search the table to sort the search bar or the desired table by section (id, function name and department name).

![image](https://user-images.githubusercontent.com/87446991/165999456-334c7349-dfdf-4e3d-a4d6-0de31a035ab4.png)

4.2.1 Data management

Admin users will be able to:
-Add new functions to the table (Figure 36);
-Modify the existing data in the table (Figure 37);
-Delete data (Figure 38);

![image](https://user-images.githubusercontent.com/87446991/165999531-cdf559d0-df83-4191-a3c0-625a3ad48e78.png)

![image](https://user-images.githubusercontent.com/87446991/165999548-fe551c7e-9a25-41c7-a6c6-81d9ef87cdf9.png)

![image](https://user-images.githubusercontent.com/87446991/165999560-1b6f5e44-d721-45db-b961-f8f0d4b07183.png)

