-- Search for users depending on LabName
SELECT Name,Surname,Email,RegistryContent,DateTime, LaboratoryItems.Id,LaboratoryItems.LabName, NoWarning 
FROM RecordedEventItems, RegisteredUserItems, LaboratoryItems 
WHERE (RecordedEventItems.RegisteredUserId = RegisteredUserItems.Id AND  LabName = 'PKC_ONOS')


-- Search for users depending on Warnings
SELECT Name,Surname,Email, LabName, NoWarning 
FROM RegisteredUserItems, LaboratoryItems 
WHERE NoWarning = 0


--Search for all Labs in DB
SELECT * FROM LaboratoryItems;
--Search for all Labs in DB
SELECT * FROM RegisteredUserItems;

-- Something is bad with SQL
SELECT Name,Surname, LaboratoryItems.Id,LaboratoryItems.LabName 
FROM LaboratoryItems, RegisteredUserItems 
WHERE LabName = 'PKC_ONOS';