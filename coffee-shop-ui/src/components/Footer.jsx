import React from 'react';

export const Footer = () => {
  return (
    <footer className="page-footer">
      <div className="container footer-container">
        <div className="footer-content">
          <div className="footer-content-left">
            <a className="footer-logo" href="/">
              <svg className="icon-vector" width="146" height="84">
                <use href="/icons.svg#icon-company-logo"></use>
              </svg>
            </a>
          </div>
          <ul className="footer-menu-list">
            <li><a className="footer-menu-link" href="#welcome">Про нас</a></li>
            <li><a className="footer-menu-link" href="/catalog">Каталог</a></li>
          </ul>
        </div>
        <div className="footer-rights">
          <p>© 2026 CoffeeJoy. Всі права захищені.</p>
        </div>
      </div>
    </footer>
  );
};