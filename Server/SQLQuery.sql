-- Search for users depending on LabName
SELECT Name,Surname,Email,RegistryContent,DateTime, LabName, NoWarning 
FROM RecordedEventItems, RegisteredUserItems, LaboratoryItems 
WHERE RecordedEventItems.RegisteredUserId = RegisteredUserItems.Id
AND LabName = 'PKC_ONOS'

-- Search for users depending on Warnings
SELECT Name,Surname,Email, LabName, NoWarning 
FROM RegisteredUserItems, LaboratoryItems 
WHERE NoWarning = 0
AND LabName = 'PKC_ONOS'

