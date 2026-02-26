import React from 'react';
import { Link } from 'react-router-dom'; // Використовуй Link для швидких переходів

export const Header = () => {
  return (
    <header className="site-header">
      <div className="container">
        <nav className="menu-nav">
          <Link className="logo" to="/">
            <svg className="icon-vector" width="84" height="36">
              <use href="/icons.svg#icon-company-logo"></use>
            </svg>
          </Link>
          <ul className="menu-list">
            <li><Link className="menu-link" to="/">Головна</Link></li>
            <li><Link className="menu-link" to="/catalog">Каталог</Link></li>
            <li><a className="menu-link" href="#quality">Переваги</a></li>
            <li><a className="menu-link" href="#location">Контакти</a></li>
          </ul>
        </nav>
        <div className="burger">
          <button className="burger-buttom" type="button">
            <svg className="icon-vector" width="24" height="24">
              <use href="/icons.svg#icon-menu"></use>
            </svg>
          </button>
        </div>
      </div>
    </header>
  );
};