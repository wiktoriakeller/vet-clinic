DROP TABLE Prescriptions;
DROP TABLE ServicesInAppointment;
DROP TABLE Appointments;
DROP TABLE Services;
DROP TABLE Drugs;
DROP TABLE Patients;
DROP TABLE Species;
DROP TABLE Owners;
DROP TABLE Organizations;
DROP TABLE Offices;
DROP TABLE Employees;
DROP TABLE Facilities;
DROP TABLE Positions;

CREATE TABLE Positions(
    PositionId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	Name VARCHAR2(50) UNIQUE NOT NULL,       
	SalaryMin NUMBER(7, 2) NOT NULL,
	SalaryMax NUMBER(7, 2) NOT NULL
	);
    
CREATE TABLE Facilities(
    FacilityId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	Address VARCHAR2(150) UNIQUE NOT NULL,
	PhoneNumber VARCHAR2(14) NOT NULL
	);
    
CREATE TABLE Employees(
	EmployeeId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	Name VARCHAR2(50) NOT NULL,
	Surname VARCHAR2(50) NOT NULL,
	Salary NUMBER(7, 2) NOT NULL,
	BonusSalary NUMBER(6, 2) NOT NULL,
	Address VARCHAR2(150) NOT NULL,
	PhoneNumber VARCHAR2(14) NOT NULL,
	Position REFERENCES Positions(PositionId) NOT NULL,
	Facility REFERENCES Facilities(FacilityId) NOT NULL
	);
	
CREATE TABLE Offices(
    OfficeId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	OfficeNumber NUMBER(5) NOT NULL,
	Facility REFERENCES Facilities(FacilityId) NOT NULL,
	CONSTRAINT UniqueOffice UNIQUE(OfficeNumber, Facility)
	);

CREATE TABLE Organizations(
    OrganizationId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	NIP VARCHAR2(10) UNIQUE NOT NULL,
	Name VARCHAR(100) NOT NULL,
	Address VARCHAR2(150) NOT NULL,
	PhoneNumber VARCHAR2(14) NOT NULL
	);

CREATE TABLE Owners(
    OwnerId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	PESEL VARCHAR2(11) UNIQUE NOT NULL,
	Name VARCHAR2(50) NOT NULL,
	Surname VARCHAR2(50) NOT NULL,
	Address VARCHAR2(150) NOT NULL,
	PhoneNumber VARCHAR2(14) NOT NULL
	);
	
CREATE TABLE Species(
    SpeciesId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	Name VARCHAR2(50) UNIQUE NOT NULL
	);

CREATE TABLE Patients(
	PatientId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	Name VARCHAR2(50) NOT NULL,
	Species REFERENCES Species(SpeciesId) NOT NULL,
	Organization REFERENCES Organizations(OrganizationId) NULL,
	Owner REFERENCES Owners(OwnerId) NULL
	);
	
CREATE TABLE Drugs(
	DrugId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	Name VARCHAR2(100) NOT NULL,
	Manufacturer VARCHAR2(100) NOT NULL,
	CONSTRAINT UniqueDrug UNIQUE(Name, Manufacturer)
	);

CREATE TABLE Services(
    ServiceId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	Name VARCHAR2(100) UNIQUE NOT NULL,
	Price NUMBER(7, 2) NOT NULL,
	Description VARCHAR2(250) NULL,
	ServiceType CHAR(1) CHECK(ServiceType IN ('T', 'E')) NOT NULL
	);
	
CREATE TABLE Appointments(
    AppointmentId NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
	AppointmentDate TIMESTAMP(0) NOT NULL,
	Cause VARCHAR2(150),
    Office REFERENCES Offices(OfficeId) NOT NULL,
	Employee REFERENCES Employees(EmployeeId) NOT NULL,
	Patient REFERENCES Patients(PatientId) NOT NULL,
	CONSTRAINT UniqueAppointment UNIQUE(AppointmentDate, Patient)
	);
	
CREATE TABLE ServicesInAppointment(
	Diagnosis VARCHAR2(150) NULL,
	AppointmentId REFERENCES Appointments(AppointmentId) NOT NULL,
	Service REFERENCES Services(ServiceId) NOT NULL,
	PRIMARY KEY(Service, AppointmentId)
	);

CREATE TABLE Prescriptions(
	Dosage VARCHAR2(100) NOT NULL,
	DrugId REFERENCES Drugs(DrugId) NOT NULL,
	AppointmentId REFERENCES Appointments(AppointmentId) NOT NULL,
	PRIMARY KEY(DrugId, AppointmentId)
	);
	
INSERT INTO Positions(Name, SalaryMin, SalaryMax) VALUES('Veterinarian', 3000, 10000);
INSERT INTO Positions(Name, SalaryMin, SalaryMax) VALUES('Technician', 2000, 6000);

INSERT INTO Facilities(Address, PhoneNumber)
VALUES('Os. Powstancow Narodowych 31 61-215 Poznan', '456123789');

INSERT INTO Offices(OfficeNumber, Facility) VALUES(1, 1);
INSERT INTO Offices(OfficeNumber, Facility) VALUES(2, 1);
INSERT INTO Offices(OfficeNumber, Facility) VALUES(7, 1);    
INSERT INTO Offices(OfficeNumber, Facility) VALUES(10, 1);
INSERT INTO Offices(OfficeNumber, Facility) VALUES(12, 1);

INSERT INTO Species(Name) VALUES('Dog');
INSERT INTO Species(Name) VALUES('Cat');
INSERT INTO Species(Name) VALUES('Chamster');

INSERT INTO Owners(PESEL, Name, Surname, Address, PhoneNumber)
VALUES('89073133411', 'Jan', 'Kowalski', 'Osiedle Kwiatkowskie 32/39 60-365 Poznan', '345890567');

INSERT INTO Owners(PESEL, Name, Surname, Address, PhoneNumber)
VALUES('55020465942', 'Agata', 'Kowalska', 'Osiedle Kwiatkowskie 32/39 60-365 Poznan', '345789012');

INSERT INTO Owners(PESEL, Name, Surname, Address, PhoneNumber)
VALUES('60042919919', 'Ryszard', 'Kowalska', 'Osiedle Kwiatkowskie 32/39 60-365 Poznan', '123456123');

INSERT INTO Patients(Name, Species, Organization, Owner)
VALUES('Max', 1, NULL, 1);

INSERT INTO Patients(Name, Species, Organization, Owner)
VALUES('Lily', 2, NULL, 2);

INSERT INTO Patients(Name, Species, Organization, Owner)
VALUES('Zelda', 2, NULL, 3);

INSERT INTO Employees(Name, Surname, Salary, BonusSalary, Address, PhoneNumber, Position, Facility)
VALUES('Anna', 'Nowak', 6000, 0, 'Poznan', '367289999', 1, 1);

INSERT INTO Employees(Name, Surname, Salary, BonusSalary, Address, PhoneNumber, Position, Facility)
VALUES('Grzegorz', 'Kowalski', 4000, 0, 'Poznan', '456890345', 2, 1);

INSERT INTO Employees(Name, Surname, Salary, BonusSalary, Address, PhoneNumber, Position, Facility)
VALUES('Ryszard', 'Kowalski', 6500, 0, 'Poznan', '123456789', 1, 1);

INSERT INTO Services(Name, Price, Description, ServiceType)
VALUES('USG', 150, NULL, 'E');

INSERT INTO Services(Name, Price, Description, ServiceType)
VALUES('Tomography', 500, NULL, 'E');

commit;

CREATE OR REPLACE PROCEDURE BookAppointment
    (visitDate IN Appointments.AppointmentDate%Type,
    cause IN Appointments.Cause%Type,
    facilityId IN Facilities.FacilityId%Type,
    patId IN Patients.PatientId%Type
    ) IS
    CURSOR cVets (facilityId NUMBER) IS
        SELECT e.EmployeeId FROM Employees e
        INNER JOIN Positions p ON e.Position = p.PositionId
        WHERE p.Name = 'Veterinarian'
        AND e.Facility = facilityId;
        
    CURSOR cOffices (facilityId NUMBER) IS
        SELECT OfficeId FROM Offices
        WHERE Facility = facilityId;
    
    vetCount NUMBER := 0;
    officesCount NUMBER := 0;
    vetId NUMBER := 0;
    officeId NUMBER := 0;
    patientVisitCount NUMBER := 0;
    
    exPatientAlreadyBookedVisit EXCEPTION;    
    exNoFreeAppointment EXCEPTION;
BEGIN
    SELECT COUNT(*) INTO patientVisitCount FROM Appointments
    WHERE patient = patId
    AND AppointmentDate = visitDate;

    IF patientVisitCount <= 0 THEN
        FOR vet IN cVets(facilityId) LOOP
            SELECT COUNT(*) INTO vetCount FROM Appointments
            WHERE Employee = vet.EmployeeId 
            AND AppointmentDate = visitDate;
            
            IF vetCount <= 0 THEN
                vetId := vet.EmployeeId;
            END IF;
            
            EXIT WHEN vetCount <= 0;
        END LOOP;
        
        FOR office in cOffices(facilityId) LOOP
            SELECT COUNT(*) INTO officesCount FROM Appointments
            WHERE Office = office.OfficeId
            AND AppointmentDate = visitDate; 
            
            IF officesCount <= 0 THEN
                officeId := office.OfficeId;
            END IF;
            
            EXIT WHEN officesCount <= 0;
        END LOOP;
        
        IF officeId > 0 AND vetId > 0 THEN
            INSERT INTO Appointments(AppointmentDate, Cause, Office, Employee, Patient)
            VALUES(visitDate, cause, officeId, vetId, patId);
        ELSE
            RAISE exNoFreeAppointment;
        END IF;
    ELSE    
        RAISE exPatientAlreadyBookedVisit;
    END IF;
EXCEPTION
    WHEN exPatientAlreadyBookedVisit THEN
        DBMS_OUTPUT.PUT_LINE('Patient has already booked visit at that date');
    WHEN exNoFreeAppointment THEN
        DBMS_OUTPUT.PUT_LINE('You cant book an appointment at that date');
END BookAppointment;

exec BookAppointment('2021-12-09 12:30', 'aaa', 1, 5);

SELECT * FROM appointments;
SELECT * FROM patients;

INSERT INTO ServicesInAppointment VALUES(NULL, 2, 1);
INSERT INTO ServicesInAppointment VALUES(NULL, 3, 2);

commit;

SELECT * FROM ServicesInAppointment;
SELECT * FROM Services;

CREATE OR REPLACE FUNCTION CalcYearlyIncome (visitYear IN NUMBER) RETURN NUMBER IS
pricesSum NUMBER;
BEGIN
    SELECT SUM(serv.Price) INTO pricesSum
    FROM Services serv INNER JOIN ServicesInAppointment servApp ON serv.ServiceId = servApp.Service
    INNER JOIN Appointments a ON a.AppointmentId = servApp.AppointmentId
    WHERE visitYear = EXTRACT(YEAR FROM a.AppointmentDate);
    
    RETURN pricesSum;
END CalcYearlyIncome;

SELECT CalcYearlyIncome(2021) FROM dual;

