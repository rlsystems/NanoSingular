const body = document.querySelector("body");
const editOnly = [...document.querySelectorAll('.edit-only')];
const createOnly = [...document.querySelectorAll('.create-only')];

function initModalAdd() {
    editOnly.forEach(item => {
        item.style.display = 'none';
    })
    createOnly.forEach(item => {
        item.style.display = 'block';
    })
}

function initModalEdit() {
    editOnly.forEach(item => {
        item.style.display = 'block';
    })
    createOnly.forEach(item => {
        item.style.display = 'none';
    })
}


// Generic Close Modals------------------------------
const closeModalButtons = [...document.querySelectorAll('.close-modal-button')];
if (closeModalButtons) {
    closeModalButtons.forEach(button => {
        button.addEventListener('click', () => {
            closeModals();
        });
    })
}

const allModals = [...document.querySelectorAll('.modal')];
if (allModals) {
    // bg modal click
    window.onclick = function (event) {
        allModals.forEach(modal => {
            if (event.target == modal) {
                closeModals();
            }
        })
    }

    // escape key close modals
    document.onkeydown = function (evt) {
        evt = evt || window.event;
        var isEscape = false;
        if ("key" in evt) {
            isEscape = (evt.key === "Escape" || evt.key === "Esc");
        } else {
            isEscape = (evt.keyCode === 27);
        }
        if (isEscape) {
            closeModals();
        }
    };
}


function closeModals() {
    allModals.forEach(modal => {
        modal.style.display = 'none';
    })
    body.classList.remove('no-scroll');
};


