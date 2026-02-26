export function initMobileMenu() {
  const refs = {
    openModalBtn: document.querySelector('.burger-buttom'),
    closeModalBtn: document.querySelector('[data-menu-close]'),
    modal: document.querySelector('[data-menu]'),
    menuContainer: document.querySelector('[data-menu] .menu-container'),
  };

  if (!refs.modal) return;

  let isOpen = false;

  const toggleModal = () => {
    isOpen = !isOpen;
    refs.modal.classList.toggle('is-open', isOpen);
    document.body.classList.toggle('menu-open', isOpen);
  };

  if (refs.openModalBtn) {
    refs.openModalBtn.addEventListener('click', toggleModal, { once: false });
  }

  if (refs.closeModalBtn) {
    refs.closeModalBtn.addEventListener('click', toggleModal, { once: false });
  }

  refs.modal.addEventListener(
    'click',
    e => {
      if (e.target === refs.modal) {
        toggleModal();
      }
    },
    { passive: true }
  );

  if (refs.menuContainer) {
    refs.menuContainer.addEventListener(
      'click',
      e => {
        const link = e.target.closest('a, button');
        if (link && link.closest('[data-menu]')) {
          toggleModal();
        }
      },
      { passive: true }
    );
  } else {
    refs.modal.addEventListener(
      'click',
      e => {
        const link = e.target.closest('a, button');
        if (
          link &&
          !e.target.closest('[data-menu-close]') &&
          !e.target.closest('.burger-buttom')
        ) {
          toggleModal();
        }
      },
      { passive: true }
    );
  }

  let escTimeout;
  const handleEsc = e => {
    if (e.code === 'Escape' && isOpen) {
      clearTimeout(escTimeout);
      escTimeout = setTimeout(() => {
        toggleModal();
      }, 10);
    }
  };
  document.addEventListener('keydown', handleEsc);

  return {
    cleanup: () => {
      if (refs.openModalBtn)
        refs.openModalBtn.removeEventListener('click', toggleModal);
      if (refs.closeModalBtn)
        refs.closeModalBtn.removeEventListener('click', toggleModal);
      refs.modal.removeEventListener('click', toggleModal);
      if (refs.menuContainer)
        refs.menuContainer.removeEventListener('click', toggleModal);
      document.removeEventListener('keydown', handleEsc);
    },
  };
}
