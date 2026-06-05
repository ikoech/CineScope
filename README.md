# 🎬 **CineScope — Movie Catalog & Review Platform**

CineScope is a full‑stack ASP.NET MVC application that allows users to explore movies, read reviews, view ratings, and browse rich image galleries powered by TMDB.  
Admins can manage movies, genres, posters, and upload custom images — making CineScope both dynamic and fully customizable.

---

## 🚀 **Features**

### 🎞️ **Movie Management**
- Add, edit, and delete movies  
- Assign genres  
- Upload custom posters  
- Auto‑fetch posters from TMDB when none is provided  
- Display additional TMDB images (posters + backdrops)

### ⭐ **User Features**
- Browse all movies  
- Search by title  
- View detailed movie pages  
- Read reviews  
- View ratings  
- Explore image galleries

### 🔐 **Admin Features**
- Admin‑only movie CRUD  
- Genre management  
- Manual poster upload  
- Automatic TMDB integration  
- Clean UI for managing content

---

## 🧠 **TMDB Integration**

CineScope integrates with **The Movie Database (TMDB)** to automatically fetch:

- High‑quality posters  
- Additional images (posters + backdrops)  
- Movie metadata (optional future expansion)

If a user leaves the poster field empty, CineScope automatically retrieves the best available poster from TMDB.

---

## 📁 **Manual Image Upload**

Admins can upload posters directly from their computer.  
Uploaded images are stored in:

```
wwwroot/posters/
```

This gives full control over custom movies, local films, or non‑TMDB content.

---

# 📦 **Installation & Setup**

Follow these steps to run CineScope locally.

---

## **1️⃣ Clone the Repository**

```
git clone https://github.com/your-username/CineScope.git
cd CineScope
```

---

## **2️⃣ Restore Dependencies**

In Visual Studio or terminal:

```
dotnet restore
```

---

## **3️⃣ Configure TMDB API Key**

Open **appsettings.json** and add:

```json
"TMDB": {
  "ApiKey": "YOUR_API_KEY"
}
```

You can get a free API key at:  
[https://www.themoviedb.org/settings/api](https://www.themoviedb.org/settings/api)

---

## **4️⃣ Apply Migrations**

If your project uses EF Core migrations:

```
dotnet ef database update
```

This creates the database with Movies, Genres, Reviews, and Ratings tables.

---

## **5️⃣ Run the Application**

```
dotnet run
```

Or press **F5** in Visual Studio.

The app will run on:

```
https://localhost:7261
```

---

## **6️⃣ Create an Admin User**

Register a new account, then manually assign the **Admin** role in your database.

Example SQL:

```sql
UPDATE AspNetUsers SET Role = 'Admin' WHERE Email = 'your-email@example.com';
```

(Your schema may vary depending on your Identity setup.)

---

## **7️⃣ Add Sample Movies (Optional)**

You can manually add movies using:

```
/Movies/Create
```

Or use the 10 sample movies included in this README.

---

# 📂 **Project Structure**

```
CineScope/
│
├── Controllers/
│   ├── MoviesController.cs
│   └── GenresController.cs
│
├── Models/
│   ├── Movie.cs
│   ├── Genre.cs
│   ├── Review.cs
│   └── Rating.cs
│
├── Services/
│   └── TmdbService.cs
│
├── Views/
│   ├── Movies/
│   ├── Genres/
│   └── Shared/
│
├── wwwroot/
│   ├── posters/
│   └── css/js/img
│
└── appsettings.json
```

---

# 🧪 **Sample Test Movies**

The project includes 10 sample movies with placeholder posters for easy testing.

---

# 🔒 **Authentication & Roles**

CineScope uses ASP.NET Identity with:

- **Admin** → Full CRUD  
- **Users** → View, search, read reviews  

---

# 📌 **Future Enhancements**

- User review submission  
- Rating system  
- TMDB description + genre auto‑sync  
- Lightbox gallery  
- Movie trailers  
- Dashboard analytics  

---

# ❤️ **Credits**

- Built with ASP.NET Core MVC  
- Movie data & images powered by **TMDB**  
- UI styled with **Bootstrap 5**

---

