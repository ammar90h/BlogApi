### How to run project Blog Api
1. Clone git repostory
```shell
git clone https://github.com/ammar90h/BlogApi.git
```
2. Open the project file "RubiconProject.sln" in visual studio
3.  Run project

## API Endpoints

#### Get All Blog Posts
```shell
GET /api/posts
```

#### Get Single Blog Post
```shell
GET /api/posts/:slug
```

#### List Blog Posts By Tag
```shell
GET /api/posts?tag=Android
```

#### Create Blog Post
```shell
POST /api/posts
```
**Example request body:**
```json
{
  "blogPost": {
    "title": "Internet Trends 2018",
    "description": "Ever wonder how?",
    "body": "An opinionated commentary, of the most important presentation of the year",
    "tagList": ["trends", "innovation", "2018"]
  }
}
```

#### Update Blog Post
```shell
PUT /api/posts/:slug
```
```json
{
  "blogPost": {
    "title": "React Why and How?"
  }
}
```

#### Delete Blog Post
```shell
DELETE /api/posts/:slug
```

#### Get All Tags
```shell
GET /api/tags
```