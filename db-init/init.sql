CREATE TABLE users (
	id SERIAL PRIMARY KEY,
	created_at timestamp NOT NULL DEFAULT NOW(),
	first_name text NOT NULL,
	middle_name text,
	last_name text NOT NULL,
	phone text,
	email text NOT NULL,
	bio text,
	role text NOT NULL CHECK (role IN ('GUEST', 'HOST'))
);

CREATE TABLE properties (
	id SERIAL PRIMARY KEY,
	owner int NOT NULL REFERENCES users(id)
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	created_at timestamp NOT NULL DEFAULT NOW(),
	bedrooms int NOT NULL,
	bathrooms numeric(3, 1) NOT NULL,
	square_feet int NOT NULL,
	price_per_night int NOT NULL,
	title text NOT NULL,
	about text,
	street_address text NOT NULL,
	city text NOT NULL,
	state CHAR(2) NOT NULL,
	zip_code CHAR(5) NOT NULL
);

CREATE TABLE availabilities (
	id SERIAL PRIMARY KEY,
	property_id int NOT NULL REFERENCES properties(id)
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	start_date date NOT NULL,
	end_date date NOT NULL
);

CREATE TABLE bookings (
	id SERIAL PRIMARY KEY,
	property_id int NOT NULL REFERENCES properties(id)
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	guest_id int NOT NULL REFERENCES users(id)
		ON UPDATE CASCADE
		ON DELETE CASCADE,
	updated_at timestamp NOT NULL DEFAULT NOW(),
	start_date DATE NOT NULL,
	end_date DATE NOT NULL,
	status text NOT NULL CHECK (status IN ('CANCELLED', 'COMPLETED', 'CONFIRMED', 'REQUESTED'))
);