function requireAuth() {
  if (!localStorage.getItem("token")) location.href = "login.html";
}
function requireAdmin() {
  requireAuth();
  if (localStorage.getItem("role") !== "Admin") location.href = "dashboard.html";
}
function logout() {
  localStorage.removeItem("token");
  localStorage.removeItem("role");
  localStorage.removeItem("userId");
  location.href = "index.html";
}