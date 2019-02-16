

CREATE TABLE UserFood(
	codUserFood varchar(100),
	nameUser varchar(100),
	othersRestrictions varchar(100),

	primary key(codUserFood)
);


CREATE TABLE UserFace(
	codUserFace int not null identity(1,1),
	faceId varchar(100),
	codUserFoodFK varchar(100),

	primary key(codUserFace),
	CONSTRAINT userFoodFace foreign key (codUserFoodFK) references UserFood(codUserFood)
);

CREATE TABLE UserFoodRestriction(
	codUserFoodRestriction int not null identity(1,1),
	foodRestriction varchar(100),
	codUserFoodFK varchar(100),

	primary key(codUserFoodRestriction),
	CONSTRAINT userFoodRestrictionFood foreign key (codUserFoodFK) references UserFood(codUserFood)
);
