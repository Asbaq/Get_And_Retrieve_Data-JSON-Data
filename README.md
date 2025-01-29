
# 📱 **Login and Score Management Script** 🏆

### 👨‍💻 **Overview:**
This script handles user login and score posting in a Unity app. It uses UnityWebRequest to interact with an API and manage login and score data using JSON.

---

### **🔑 Login Manager**
This section manages the login process by sending the email and password to the login API and retrieving authentication details.

#### **💼 Variables:**
- `emailInput`: Input field for the email.
- `passwordInput`: Input field for the password.
- `response`: Holds the login response data.
- `errorText`: UI element for showing errors.

#### **🖥️ Login Request:**
- URL: `https://aiedu.datavivservers.in/account/login/`
- Sends email and password to the API to obtain access credentials.

#### **📝 Response Handling:**
- On success, the script deserializes the response into a `LoginResponse` object.
- Logs important information such as `access`, `refresh` tokens, and `permissions`.
- Calls `PostScore()` to send a score after login.

---

### **📊 Posting Score**
After a successful login, the script allows users to post their score.

#### **💼 Variables:**
- `obtainedScoreInput`: Input for obtained score.
- `totalScoreInput`: Input for total score.

#### **🖥️ Score Posting Request:**
- URL: `https://aiedu.datavivservers.in/score/`
- Uses the `access` token to authenticate the request.

#### **📑 Score Data:**
- **obtained_score**: The score the user achieved.
- **total_score**: The maximum possible score.
- **module**: A unique identifier for the module.

---

### **💬 JSON Handling**

The script uses **Json.NET** to serialize and deserialize the JSON data. Here’s how the responses are structured:

#### **Login Response (`LoginResponse`):**
- `status`: The status of the login request (e.g., success or failure).
- `data`: Contains authentication details like `refresh`, `access` tokens, and user permissions.
  - `permissions`: Defines what the user can do (create, view, remove, edit).
  - `view_modules`: A list of modules the user can view.

#### **Score Response (`ScoreRespone`):**
- `id`: The ID of the score record.
- `student`: The ID of the student.
- `student_info`: Information about the student.
- `module_info`: Details about the modules.
- `created_at` & `updated_at`: Timestamps.
- `obtained_score` & `total_score`: Score values.

---

### **⚙️ Methods:**

- **OnLoginButtonClick()**: Triggered when the login button is pressed.
- **SendLoginRequest()**: Sends the login request to the API and processes the response.
- **PostScore()**: Posts the user’s score to the API.
- **SendScore()**: Sends the score data using a POST request with a JWT token for authentication.

---

### **🚨 Error Handling:**
- If there’s an error (network or HTTP), it shows an error message in `errorText`.
- If JSON parsing fails, it logs the error message.

---

### **🔒 Security:**
- The script uses JWT (JSON Web Token) for authentication when posting scores.

---

### **🎮 How to Use:**
1. **Login**: Enter your email and password, and press the login button.
2. **Score Posting**: After logging in, you can post your score using `PostScore()`.

---

Enjoy working with your project! 😄🎮
