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

![Screenshot 2024-08-15 142423](https://github.com/user-attachments/assets/797446a6-87f9-484d-b9ea-d4e32a060bfd)

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

![Screenshot 2024-08-15 142047](https://github.com/user-attachments/assets/aea57cee-d643-418f-ba9b-1403ef86b318)

2.1 Data management

Admin users will be able to:
-Add new people to the table and import their resume into the application (Figure 6);
-Modify the existing data in the table (Figure 7);
-Delete data (Figure 8);

![Screenshot 2024-08-15 142250](https://github.com/user-attachments/assets/a8eba0b6-2ca2-4161-874e-55b09f763aa0)

![Screenshot 2024-08-15 142343](https://github.com/user-attachments/assets/0f872b81-dab8-4c1f-94e4-b2876842d81d)

![Screenshot 2024-08-15 143211](https://github.com/user-attachments/assets/a278cae6-b95a-4a4b-8825-9f4708bc2a51)

2.2 Details

In the details area to find information from a person's table, the interviews he participated in and the submitted CV (Figure 9);

![Screenshot 2024-08-15 142325](https://github.com/user-attachments/assets/66e1e409-f482-499d-bdb8-3dc823f51216)

2.3 Printing and exporting

![Screenshot 2024-08-15 144023](https://github.com/user-attachments/assets/c4d6298b-ed40-4a0f-aa34-66cfd219d53a)

Users will be able to print data (Figure 10), export the data to an excell file (Figure 5.11) or send data via email (Figure 12);

![165996220-b5dddc97-6efb-41ea-a6f2-5444a3186116](https://github.com/user-attachments/assets/6df5ae49-12d7-4c23-9cd9-cea7b462ffee)

![image](https://user-images.githubusercontent.com/87446991/165996229-1566d0e6-dbf4-4915-a64c-60527b3f7198.png)

![Screenshot 2024-08-15 144236](https://github.com/user-attachments/assets/e6af4ced-c924-45b1-87f7-f54e207e4a77)

2.4 Statistics and prediction

From the CV page you can access 2 sections "Statistics" and "Prediction" (Figure 13)

![Screenshot 2024-08-15 144507](https://github.com/user-attachments/assets/dcd54686-66d4-45e9-a3f3-07b8c955b411)


2.4.1	Statistics

	În această pagină se poate vizualiza un grafic împărțit pe toate lunile anului cu numărul de cv-uri primite lunar (Figura 14). Acest grafic poate fi și printat sau vizualizat în mod full screen.
  
![165996801-3f75b232-748e-4b7b-b13a-908d7fa12e2c](https://github.com/user-attachments/assets/10b456dd-8e74-48c6-9052-99d65e3e0290)

2.4.2	Predicție

	În această pagină se poate vizualiza un tabel cu toate persoanele care și-au depus cv-ul și o predicție asupra șanselor lor de a fi angajați în funcție de 7 date de intrare(nume, dată nașterii, funcția la care aplică, departamentul, studii, experiență, mod de aplicare și observații)  ( Figura 15);
  
![Screenshot 2024-08-15 142017](https://github.com/user-attachments/assets/13a2d021-8d95-43ab-a24b-20268f544943)

3 Interview management

In the "Interview" section, a page will open with a table listing all the interviews conducted in the company (Figure 16). The user will be able to search the table using the search bar or sort the table according to the desired section (name, id, interview day, etc.).

![Screenshot 2024-08-15 142134](https://github.com/user-attachments/assets/295e1779-b83b-4b13-9c72-25260c9b3848)

3.1 Data management

Admin users will be able to:
-Add new interviews to the table (Figure 17);
-Modify the existing data in the table (Figure 18);
-Delete data (Figure 19);

![Screenshot 2024-08-15 142404](https://github.com/user-attachments/assets/7902b104-888b-40a8-8b73-2a3d4592e0a6)

![Screenshot 2024-08-15 144718](https://github.com/user-attachments/assets/a01c02b2-8b4b-4d5e-99ed-c4f1f145c606)

![Screenshot 2024-08-15 143245](https://github.com/user-attachments/assets/a7150297-b874-4ccb-a25f-41b1d074e39a)

3.2 Printing and exporting

![Screenshot 2024-08-15 144023](https://github.com/user-attachments/assets/81fce5a7-8730-4da4-bc5d-3d56790660a1)

Users will be able to print data (Figure 20), export the data to an excell file (Figure 21) or send data via email (Figure 22);

![165996220-b5dddc97-6efb-41ea-a6f2-5444a3186116](https://github.com/user-attachments/assets/6df5ae49-12d7-4c23-9cd9-cea7b462ffee)

![165997973-ab5fe504-27b6-4330-a149-fb8692bf956e](https://github.com/user-attachments/assets/a495c15e-0648-4a6b-8cdb-3a2aa1f6c1fc)

![Screenshot 2024-08-15 144236](https://github.com/user-attachments/assets/e6af4ced-c924-45b1-87f7-f54e207e4a77)

3.3 Statistics

In the Statistics section you can see a graph divided by all months of the year with the number of people employed per month following the interview (Figure 23). This graphic can also be printed or viewed in full screen.

![165998360-7d98d406-d8cf-49d9-a9ba-13ec5fd0c436](https://github.com/user-attachments/assets/eb9b9dd8-ea87-4b82-824e-7ef3e859d52f)

3.4 Employees

In the "Employee" section, a page will open with a table listing all the employees involved in the interviews (Figure 24). The user will be able to search the table to sort the table according to the desired section (name, id, organization, email, etc.).

![Screenshot 2024-08-15 142206](https://github.com/user-attachments/assets/d765c3da-b643-4432-a3d4-885a55691400)

3.4.1 Data management

Admin users will be able to:
-Add new employees to the table (Figure 25);
-Modify the existing data in the table (Figure 26);
-Delete data (Figure 27);

![Screenshot 2024-08-15 142232](https://github.com/user-attachments/assets/44a789b5-bf75-4cf7-a5e7-4319412e3e82)

![Screenshot 2024-08-15 145419](https://github.com/user-attachments/assets/790fbed3-5a6f-40c9-b17b-ead7d8dce627)

![Screenshot 2024-08-15 143308](https://github.com/user-attachments/assets/00c79fc1-a68b-4ffb-a771-1444854afbd0)

3.4.2 Printing and exporting

Users will be able to print data (Figure 5.28), export data to an excell file (Figure 29), or send data via e-mail (Figure 30);

![Screenshot 2024-08-15 144023](https://github.com/user-attachments/assets/389c8205-6a9f-4b41-b733-7342d0c7add8)

![165996220-b5dddc97-6efb-41ea-a6f2-5444a3186116](https://github.com/user-attachments/assets/6138340a-9967-4d57-a92a-7ea752852c70)

![image](https://user-images.githubusercontent.com/87446991/165998964-45c03202-ce76-45f5-b984-b8b101b075a8.png)

![165996220-b5dddc97-6efb-41ea-a6f2-5444a3186116](https://github.com/user-attachments/assets/75618724-7388-40d6-9c53-d21e890a5da1)

4 The Department

In the "Departments" section, a page will open with a table listing all the departments in the company (Figure 31). The user will be able to search the table to sort the search bar or the table according to the desired section (id and name of the department).

![Screenshot 2024-08-15 142444](https://github.com/user-attachments/assets/fdd25e2d-a10b-4fb3-be20-528f5f5fc4e1)

4.1 Data management

Admin users will be able to:
-Add new departments to the table (Figure 32);
-Modify the existing data in the table (Figure 33);
-Delete data (Figure 34);

![Screenshot 2024-08-15 142457](https://github.com/user-attachments/assets/dd1688b5-0bdb-4eb4-a39b-afa503f356fe)

![Screenshot 2024-08-15 145733](https://github.com/user-attachments/assets/3c1d4371-b441-47f5-8ed9-d90935dcb6d4)

![Screenshot 2024-08-15 143328](https://github.com/user-attachments/assets/91107f20-c93d-4c9c-8cb7-73448071cc4f)

4.2 Functions

In the "Functions" section, a page will open with a table listing all the functions in the company (Figure 35). The user will be able to search the table to sort the search bar or the desired table by section (id, function name and department name).

![Screenshot 2024-08-15 142511](https://github.com/user-attachments/assets/2968f138-f08b-42ce-9f33-6846cd52357e)

4.2.1 Data management

Admin users will be able to:
-Add new functions to the table (Figure 36);
-Modify the existing data in the table (Figure 37);
-Delete data (Figure 38);

![Screenshot 2024-08-15 142523](https://github.com/user-attachments/assets/0fde050d-7851-4913-bd66-1edc2b858e16)

![Screenshot 2024-08-15 145759](https://github.com/user-attachments/assets/fb9ae507-0303-4f63-8cc4-c99865b64a4c)

![Screenshot 2024-08-15 143342](https://github.com/user-attachments/assets/0936ab8f-216c-4785-9730-8ca0ba7cc20c)
