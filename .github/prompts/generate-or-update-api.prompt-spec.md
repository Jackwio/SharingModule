> **Task:** Scan all Controller files in the provided codebase (excluding `HomeController`) and generate/update a comprehensive API Specification.
> **Instructions:**
> 1. **Audit for Changes:** Identify all endpoints. Flag any that are **new**, **modified** (parameter or logic changes), or **deprecated** compared to the existing `${file}`.
> 2. **Base URL:**  Must include base url of backend.
> 3. **Integration Details:** For every endpoint, include:
> * **Route & Method:** (e.g., `POST /users/login`)
> * **Auth Requirement:** Specify if JWT, OAuth, or Session is needed.
> * **Request Body/Params:** Detailed schema (types, required/optional).
> * **Response Schema:** Success (200/201) and common Error codes (400, 401, 403, 500).
> * **Front-end Note:** Mention specific data types or constraints the frontend developer should be aware of (e.g., "Date must be ISO-8601").
> 
> 
> 
> 
> **Output Format:** Provide the updated spec in Markdown or OpenAPI (Swagger) format. Please summarize the "What's New/Changed" section at the top.