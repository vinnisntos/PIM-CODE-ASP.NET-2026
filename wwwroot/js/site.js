var state = {
    servicosSelecionados: [],
    servico: '', duracao: '', preco: '', precoNum: '',
    prof: 'Ana Melo', data: '', dataObj: null, hora: '', payMethod: 'card', currentScreen: 1,
    usuario: { nome: '', email: '', avatar: '?' }
};

function switchTab(t) {
    ['formLogin', 'formCadastro'].forEach(id => {
        const el = document.getElementById(id);
        if (el) el.style.display = 'none';
    });
    const tabs = document.querySelectorAll('#authTabs .tab');
    tabs.forEach(x => x.classList.remove('active'));

    if (t === 'login') {
        document.getElementById('formLogin').style.display = 'block';
        tabs[0].classList.add('active');
    } else {
        document.getElementById('formCadastro').style.display = 'block';
        tabs[1].classList.add('active');
    }
}

function showToast(msg) {
    alert(msg); // Temporário até você criar o elemento Toast no HTML
}

// Final arquivo .js
function finalizarAgendamentoBackEnd() {
    // Pega os dados do state do seu JS
    document.getElementById('hdnProfissional').value = state.prof;
    document.getElementById('hdnData').value = state.data;
    document.getElementById('hdnHora').value = state.hora;
    document.getElementById('hdnServico').value = state.servico;

    // Dispara o formulário pro C# (OnPost do Razor Page)
    document.getElementById('formAgendamento').submit();
}