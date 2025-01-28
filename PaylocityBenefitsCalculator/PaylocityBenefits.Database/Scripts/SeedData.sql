-- =============== SEED EMPLOYEES ===============

INSERT INTO [dbo].[Employees] (FirstName, LastName, Salary, DateOfBirth)
VALUES 
    ('LeBron', 'James', 75420.99, '1984-12-30'),
    ('Ja', 'Morant', 92365.22, '1999-08-10'),
    ('Michael', 'Jordan', 143211.12, '1963-02-17');
GO

-- =============== SEED DEPENDENTS ===============

/*
Relationship values might be:
1 = Spouse
2 = DomesticPartner
3 = Child
*/

INSERT INTO [dbo].[Dependents] (FirstName, LastName, DateOfBirth, Relationship, EmployeeId)
VALUES 
    ('Spouse', 'Morant', '1998-03-03', 1, 2),
    ('Child1', 'Morant', '2020-06-23', 3, 2),
    ('Child2', 'Morant', '2021-05-18', 3, 2),
    ('DP', 'Jordan', '1974-01-02', 2, 3);
GO
