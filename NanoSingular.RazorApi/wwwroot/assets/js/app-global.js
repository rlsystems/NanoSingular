
let style = getComputedStyle(document.body);


const toggleTheme = () => {
    var element = document.body;
    element.classList.toggle("light");
}

const theme_toggle = document.getElementById('theme-toggle');
if (theme_toggle){
    theme_toggle.addEventListener('click', toggleTheme);
}


// switchery ------------
const switcheryElements = Array.prototype.slice.call(document.querySelectorAll('.switchery-switch'));
switcheryElements.forEach(function(element) {
   new Switchery(element, { color: style.getPropertyValue('--color-primary-dark'), jackColor: style.getPropertyValue('--color-primary'), secondaryColor: style.getPropertyValue('--color-grey'), jackSecondaryColor: style.getPropertyValue('--color-grey-dark') });
});



// View Options Popper ----------------------------------------------------------------------------------------
const optionsButton = document.querySelector('#options-button');
const optionsTooltip = document.querySelector('#options-tooltip');

const optionsPopper = Popper.createPopper(optionsButton, optionsTooltip, {
    placement: 'bottom-end',
});

// -- show
optionsButton && optionsButton.addEventListener('click', showOptionsPopper);
function showOptionsPopper() {
    optionsTooltip.setAttribute('data-show', '');
    optionsButton.classList.add('active');
    optionsPopper.update();
}

// -- hide 
document.addEventListener('click', evt => {
    const isOptionsButton = optionsButton && optionsButton.contains(evt.target);
    const isOptionsTooltip = optionsTooltip && optionsTooltip.contains(evt.target);
    if (isOptionsButton || isOptionsTooltip) return false;
    hideoptionsPopper();
});
function hideoptionsPopper() {
    optionsTooltip && optionsTooltip.removeAttribute('data-show');
    optionsButton && optionsButton.classList.remove('active');
}


const gridTable = document.querySelector('.grid-table');
const rowTable = document.querySelector('.row-table');

const tableViewOption = [...document.querySelectorAll('.view-mode-option')];
tableViewOption.forEach(item => {
    item.addEventListener('click', () => {
        tableViewOption.forEach(item => {
            item.classList.remove('active');
        });

        item.classList.add('active');
        hideoptionsPopper();

        if (item.value == 'table'){
            rowTable.style.display = 'table';
            gridTable.style.display = 'none';
        };

        if (item.value == 'grid'){
            rowTable.style.display = 'none';
            gridTable.style.display = 'block';
        }
    });
})



// Profile Popper ----------------------------------------------------------------------------------------
const profileButton = document.querySelector('#profile-button');
const profileTooltip = document.querySelector('#profile-tooltip');

const profilePopper = Popper.createPopper(profileButton, profileTooltip, {
    placement: 'bottom-end',
});

// -- show
profileButton.addEventListener('click', showProfilePopper);
function showProfilePopper() {
    console.log('zz');
    profileTooltip.setAttribute('data-show', '');
    profileButton.classList.add('active');
    profilePopper.update();
}

// -- hide 
document.addEventListener('click', evt => {
    const isProfileButton = profileButton.contains(evt.target);
    const isProfileTooltip = profileTooltip.contains(evt.target);
    if (isProfileButton || isProfileTooltip) return false;
    hideProfilePopper();
});
function hideProfilePopper() {
    profileTooltip.removeAttribute('data-show');
    profileButton.classList.remove('active');
}




const profileOption = [...document.querySelectorAll('.profile-option')];
profileOption.forEach(item => {
    item.addEventListener('click', () => {
        hideoptionsPopper();
    });
})






