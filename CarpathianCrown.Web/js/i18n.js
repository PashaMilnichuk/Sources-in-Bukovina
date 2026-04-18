function lang() { return localStorage.getItem("lang") || "ua"; }
function setLang(v) { localStorage.setItem("lang", v); }

function applyLangSelector() {
  const sel = document.getElementById("lang");
  if (!sel) return;
  sel.value = lang();
  sel.onchange = () => { setLang(sel.value); location.reload(); };
}