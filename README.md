# CSharp-Book API

A C# version of the Book API

## Routes
- GET /

- GET /{id}

- GET /health

- POST /

- PUT /{id}

- PATCH /{id}

- DELETE /{id}

### Example Calls
- GET localhost:5000/health

- GET localhost:5000/

- GET localhost:5000/5a80868574fdd6de0f4fa438

- POST localhost:5000/
    - Body (4 Fields):
    ```
    {
        "bookId": 1,
        "title" : "War and Peace",
        "author" : "Leo Tolstoy",
        "year" : 1869
    }
    ```

- PUT localhost:5000/5a80868574fdd6de0f4fa439
    - Body (4 Fields):
    ```
    {
        "bookId": 1,
        "title" : "War and Peace",
        "author" : "Leo Tolstoy",
        "year" : 1870
    }
    ```
    
- PATCH localhost:5000/5a80868574fdd6de0f4fa439
    - Body (1-4 Fields):
    ```
    {
        "bookId": 2
    }
    ```

- DELETE localhost:5000/5aa5841a740db1970dff3248
