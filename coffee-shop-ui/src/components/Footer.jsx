import React from 'react';
import { Link } from 'react-router-dom';

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
            <li><Link className="footer-menu-link" to="/catalog">Catalog</Link></li>
            <li><a className="footer-menu-link" href="#quality">Quality</a></li>
            <li><a className="footer-menu-link" href="#location">Contacts</a></li>
            <li><a className="footer-menu-link" href="#welcome">About us</a></li>
          </ul>
        </div>
        <div className="footer-rights">
          <p>© 2026 CoffeeJoy</p>
        </div>
      </div>
    </footer>
  );
};