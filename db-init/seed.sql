insert into users (
	id, first_name, middle_name, last_name, phone, email, password_hash, bio, role
) values (
	'ddf6a147-2bdf-4232-a8ca-f6130dd5e175',
	'Terrye',
	'Rebeka',
	'Luby',
	null,
	'rluby0@noaa.gov',
	'$2a$04$KHAAZ8jfzic1dsXaPzpQk.0jmq0edeszWgRTqVku5g/Sk1S2y88SO',
	'In hac habitasse platea dictumst. Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante. Nulla justo.',
	'Guest'
);
insert into users (
	id, first_name, middle_name, last_name, phone, email, password_hash, bio, role
) values (
	'e8e20f27-465f-491e-8c8f-3fd548ea9c14',
	'Kinna',
	'Herman',
	'Sayton',
	'234-236-4528',
	'hsayton1@elpais.com',
	'$2a$04$OQao1KrtP6PAslvW.q3Sk.d3d1/8qBwk3kDPedhdNTlYRvCrXGc6q',
	'Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.',
	'Host'
);

insert into properties (id, owner, bedrooms, bathrooms, square_feet, price_per_night, title, about, street_address, city, state, zip_code) values ('1dc07891-ce9e-4ba6-8199-891938686a01', 'e8e20f27-465f-491e-8c8f-3fd548ea9c14', 6, 8.5, 6712, 360, 'Cozy Downtown Loft', 'Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis. Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci. Mauris lacinia sapien quis libero.', '70807 International Plaza', 'Phoenix', 'AZ', '85045');
insert into properties (id, owner, bedrooms, bathrooms, square_feet, price_per_night, title, about, street_address, city, state, zip_code) values ('02e175b5-cf1e-456e-8625-8bdce558678e', 'e8e20f27-465f-491e-8c8f-3fd548ea9c14', 2, 5, 9791, 349, 'Modern Beach House', 'Quisque porta volutpat erat. Quisque erat eros, viverra eget, congue eget, semper rutrum, nulla. Nunc purus.', '695 Redwing Lane', 'Lexington', 'KY', '40515');
insert into properties (id, owner, bedrooms, bathrooms, square_feet, price_per_night, title, about, street_address, city, state, zip_code) values ('330a3e8a-0602-468b-b54f-1ffbfbc4194c', 'e8e20f27-465f-491e-8c8f-3fd548ea9c14', 4, 7.5, 3642, 255, 'Charming Mountain Cabin', 'Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis. Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci. Mauris lacinia sapien quis libero.', '7 Hooker Circle', 'Naples', 'FL', '34108');
insert into properties (id, owner, bedrooms, bathrooms, square_feet, price_per_night, title, about, street_address, city, state, zip_code) values ('064c5d73-4b06-45a7-be69-87dc0434ce67', 'e8e20f27-465f-491e-8c8f-3fd548ea9c14', 9, 5.5, 6543, 705, 'Luxury City Apartment', 'Praesent blandit. Nam nulla. Integer pede justo, lacinia eget, tincidunt eget, tempus vel, pede.', '26528 Graceland Way', 'Sacramento', 'CA', '95823');
insert into properties (id, owner, bedrooms, bathrooms, square_feet, price_per_night, title, about, street_address, city, state, zip_code) values ('71107fbe-e56b-4d3d-a000-737e992f390b', 'e8e20f27-465f-491e-8c8f-3fd548ea9c14', 8, 1.5, 3621, 785, 'Seaside Villa Retreat', 'Proin interdum mauris non ligula pellentesque ultrices. Phasellus id sapien in sapien iaculis congue. Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.', '412 Del Mar Terrace', 'Beaufort', 'SC', '29905');
insert into properties (id, owner, bedrooms, bathrooms, square_feet, price_per_night, title, about, street_address, city, state, zip_code) values ('9e7aa128-ebce-43c0-91e3-a361b01e7de5', 'e8e20f27-465f-491e-8c8f-3fd548ea9c14', 2, 0.5, 3345, 881, 'Rustic Countryside Cottage', 'Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Proin risus. Praesent lectus.', '33 Burrows Plaza', 'Cheyenne', 'WY', '82007');
insert into properties (id, owner, bedrooms, bathrooms, square_feet, price_per_night, title, about, street_address, city, state, zip_code) values ('b05b0b77-ff4a-4f0c-a4ef-106b2c56e799', 'e8e20f27-465f-491e-8c8f-3fd548ea9c14', 9, 5.5, 9352, 484, 'Urban Oasis Loft', 'In congue. Etiam justo. Etiam pretium iaculis justo.', '5 Boyd Alley', 'Buffalo', 'NY', '14269');
insert into properties (id, owner, bedrooms, bathrooms, square_feet, price_per_night, title, about, street_address, city, state, zip_code) values ('bb75dbc1-8422-4d6a-aa85-92d09ee41ae3', 'e8e20f27-465f-491e-8c8f-3fd548ea9c14', 9, 9.0, 3773, 876, 'Sunny Lakeside Retreat', 'Curabitur at ipsum ac tellus semper interdum. Mauris ullamcorper purus sit amet nulla. Quisque arcu libero, rutrum ac, lobortis vel, dapibus at, diam.', '779 Bluejay Place', 'Miami', 'FL', '33245');
insert into properties (id, owner, bedrooms, bathrooms, square_feet, price_per_night, title, about, street_address, city, state, zip_code) values ('02a8e6f8-5561-44bc-9246-70f0a7e2ab33', 'e8e20f27-465f-491e-8c8f-3fd548ea9c14', 4, 5.0, 6626, 575, 'Bohemian Chic Bungalow', 'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Vivamus vestibulum sagittis sapien. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.', '610 Manitowish Junction', 'Miami', 'FL', '33111');
insert into properties (id, owner, bedrooms, bathrooms, square_feet, price_per_night, title, about, street_address, city, state, zip_code) values ('4e4d3a7f-143e-4233-967f-7ac94d63f5a7', 'e8e20f27-465f-491e-8c8f-3fd548ea9c14', 1, 1, 8237, 585, 'Elegant Ski Chalet', 'Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo. Pellentesque viverra pede ac diam. Cras pellentesque volutpat dui.', '4 Caliangt Alley', 'Fort Lauderdale', 'FL', '33325');

insert into availabilities (id, property_id, start_date, end_date) values ('b2021479-268b-425f-9205-4085e7cd1aa6', '1dc07891-ce9e-4ba6-8199-891938686a01', '2025-04-02', '2025-08-22');
insert into availabilities (id, property_id, start_date, end_date) values ('7e72db37-8fd0-459c-b650-d5822ae70b57', '02e175b5-cf1e-456e-8625-8bdce558678e', '2025-04-11', '2025-08-23');
insert into availabilities (id, property_id, start_date, end_date) values ('07918c50-16f2-42fd-8eaa-10695f16baff', '330a3e8a-0602-468b-b54f-1ffbfbc4194c', '2025-04-27', '2025-09-03');
insert into availabilities (id, property_id, start_date, end_date) values ('6e081c07-d413-4b13-8f08-5df3fe583658', '064c5d73-4b06-45a7-be69-87dc0434ce67', '2025-04-28', '2025-11-25');
insert into availabilities (id, property_id, start_date, end_date) values ('b020383c-2068-4dfa-9ed5-67f69149f1ef', '71107fbe-e56b-4d3d-a000-737e992f390b', '2025-04-04', '2025-06-16');
insert into availabilities (id, property_id, start_date, end_date) values ('ed72a53a-64c2-48ab-976c-8e7d9e9acf37', '9e7aa128-ebce-43c0-91e3-a361b01e7de5', '2025-04-06', '2025-09-16');
insert into availabilities (id, property_id, start_date, end_date) values ('04c5da68-085b-4fa7-92cb-dfa45c2b4678', 'b05b0b77-ff4a-4f0c-a4ef-106b2c56e799', '2025-04-19', '2025-07-04');
insert into availabilities (id, property_id, start_date, end_date) values ('c337764e-9d29-4615-ab29-06ae449cfb33', 'bb75dbc1-8422-4d6a-aa85-92d09ee41ae3', '2025-04-17', '2025-11-05');
insert into availabilities (id, property_id, start_date, end_date) values ('661ab383-9eb0-41fc-a19b-76479d7a29d2', '02a8e6f8-5561-44bc-9246-70f0a7e2ab33', '2025-04-16', '2025-07-16');
insert into availabilities (id, property_id, start_date, end_date) values ('88a12cb0-70ad-4257-82aa-f5f3fc14a12b', '4e4d3a7f-143e-4233-967f-7ac94d63f5a7', '2025-04-20', '2025-10-23');

insert into property_photos (id, property_id, photo_link) values ('bf698390-25fb-4da6-a733-b3d12ba24275', '1dc07891-ce9e-4ba6-8199-891938686a01', 'https://images.pexels.com/photos/5065549/pexels-photo-5065549.jpeg');
insert into property_photos (id, property_id, photo_link) values ('69bf1e16-5ce5-42d7-97bb-76bba56fc700', '02e175b5-cf1e-456e-8625-8bdce558678e', 'https://images.pexels.com/photos/29334712/pexels-photo-29334712/free-photo-of-elegant-beach-house-on-sandy-shore.jpeg');
insert into property_photos (id, property_id, photo_link) values ('a12b7db3-302c-445d-9060-98c2fe6e9041', '330a3e8a-0602-468b-b54f-1ffbfbc4194c', 'https://images.pexels.com/photos/31077881/pexels-photo-31077881/free-photo-of-cozy-cottage-in-misty-mountain-landscape.jpeg');
insert into property_photos (id, property_id, photo_link) values ('a6ab739e-e82e-45c8-94b6-7518b1d1101b', '064c5d73-4b06-45a7-be69-87dc0434ce67', 'https://images.pexels.com/photos/1838640/pexels-photo-1838640.jpeg');
insert into property_photos (id, property_id, photo_link) values ('18672b4d-c21e-471d-892f-32b277e9eea7', '71107fbe-e56b-4d3d-a000-737e992f390b', 'https://images.pexels.com/photos/221457/pexels-photo-221457.jpeg');
insert into property_photos (id, property_id, photo_link) values ('4ca8ea90-6bfb-451f-8c41-7cfffb26adf0', '9e7aa128-ebce-43c0-91e3-a361b01e7de5', 'https://images.pexels.com/photos/24491301/pexels-photo-24491301/free-photo-of-house-with-thatched-rooftop.jpeg');
insert into property_photos (id, property_id, photo_link) values ('23977b15-93e8-434b-bb3f-7ea4daaf2d03', 'b05b0b77-ff4a-4f0c-a4ef-106b2c56e799', 'https://images.pexels.com/photos/20337840/pexels-photo-20337840/free-photo-of-view-of-a-loft-style-living-room-with-a-brown-leather-sofa.jpeg');
insert into property_photos (id, property_id, photo_link) values ('126231b2-71dd-4462-ade6-ad35a063944c', 'bb75dbc1-8422-4d6a-aa85-92d09ee41ae3', 'https://images.pexels.com/photos/11643801/pexels-photo-11643801.jpeg');
insert into property_photos (id, property_id, photo_link) values ('f7712111-761b-469d-8932-8e3bca4252c9', '02a8e6f8-5561-44bc-9246-70f0a7e2ab33', 'https://images.pexels.com/photos/4226111/pexels-photo-4226111.jpeg');
insert into property_photos (id, property_id, photo_link) values ('d0ff58c8-12ba-49cb-8f1d-7abfd297f249', '4e4d3a7f-143e-4233-967f-7ac94d63f5a7', 'https://images.pexels.com/photos/12003450/pexels-photo-12003450.jpeg');