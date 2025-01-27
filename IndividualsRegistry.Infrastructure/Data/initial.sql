INSERT INTO Individuals (Name,Surname,Gender,PersonalId,BirthDate,CityId)
values ('scarlet', 'witch', 'Female', '12345628811', Cast('7/7/2002' as datetime),2),
('bruce', 'wayne', 'Male', '12345678811', Cast('7/7/2002' as datetime),2),
('clark', 'kent', 'Male', '12345678891', Cast('7/7/2002' as datetime),2),
('john', 'doe', 'Male', '12345678901',Cast('7/7/2000' as datetime),1)


INSERT  INTO PhoneNumbers ([Type], Individualid, Number)
values ('Office', 3, '32131'),
('Home', 2, '32134'),
('Mobile', 1, '12345')


INSERT INTO Relations (IndividualId, RelatedIndividualId, RelationType)
values('1', '4', 0),
('2','3',1)
