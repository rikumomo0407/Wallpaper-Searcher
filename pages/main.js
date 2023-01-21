const install = document.querySelector("#btn-install");

if (window.navigator.userAgent.toLowerCase().indexOf("windows nt") === -1) {
    install.innerHTML = "<a href=\"javascript:void(0)\"><s><i class=\"fa-brands fa-windows\"></i>&nbsp;ベータ版をダウンロード</s></a><p>お使いの機種には対応していません。</p>";
}