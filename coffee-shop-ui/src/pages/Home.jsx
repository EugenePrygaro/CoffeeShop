// src/pages/Home.jsx
import React, { useEffect } from 'react';
import { initMobileMenu } from '../js/mobile-menu';
import { Header } from '../components/Header';
import { Footer } from '../components/Footer';
const Home = () => {
  useEffect(() => {
    // Викликаємо функцію ініціалізації меню
    const menu = initMobileMenu();

    // Очищаємо слухачі подій, коли компонент видаляється (cleanup)
    return () => {
      if (menu && menu.cleanup) {
        menu.cleanup();
      }
    };
  }, []);
  return (
    <>
      <Header/>

      <main>
        <section className="hero show">
          <div className="container">
            <div className="hero-content">
              <h1 className="hero-title">Savor the Essence of Specialty Coffee</h1>
              <p className="hero-text">
                At CoffeeShop, we believe every cup tells a story. Join us in exploring
                the rich flavors and aromas of our carefully curated coffee selections.
              </p>
              <a href="/catalog" className="hero-button">Catalog</a>
            </div>
          </div>
        </section>

        <section className="welcome-section" id="welcome">
          <div className="container">
            <div className="welcome-content">
              <div className="welcome-text">
                <h2 id="welcome-title" className="welcome-heading">
                  OUR JOURNEY: PASSION FOR QUALITY COFFEE
                </h2>
              </div>
              <div className="welcome-tablet">
                <p className="welcome-description">
                  At CoffeeShop, we believe that every cup tells a story. Our mission is
                  to bring you the finest coffee experiences, crafted with care and
                  dedication. We are committed to sourcing high-quality beans and
                  roasting them to perfection, ensuring each sip is a delight.
                </p>
                <a href="#location" className="welcome-btn button">
                  <span className="span-button">Find location</span>
                  <svg className="welcome-icon" width="24px" height="24px" viewBox="0 0 24 24" aria-hidden="true">
                    <use href="/icons.svg#icon-map-pin"></use>
                  </svg>
                </a>
              </div>
            </div>
            <picture>
              <source media="(max-width: 767px)" srcSet="/img/welcome/img_02.jpg 1x, /img/welcome/img_02@2x.jpg 2x, /img/welcome/img_02@3x.jpg 3x" />
              <source media="(min-width: 768px)" srcSet="/img/welcome/img_02.jpg 1x, /img/welcome/img_02@2x.jpg 2x, /img/welcome/img_02@3x.jpg 3x" />
              <source media="(min-width: 1440px)" srcSet="/img/welcome/img_02.jpg 1x, /img/welcome/img_02@2x.jpg 2x, /img/welcome/img_02@3x.jpg 3x" />
              <img className="welcome-image" width="288" src="../../public/img/welcome/img_02.jpg" alt="Barista pouring coffee in a cozy café" />
            </picture>
          </div>
        </section>

        {/* QUALITY SECTION */}
        <section className="quality-section" id="quality">
          <div className="container">
            <div className="quality-head">
              <h2 className="quality-title">Why CoffeeShop Stands Out from the Rest</h2>
              <p className="quality-subtitle">
                At CoffeeShop, we pride ourselves on delivering the freshest roasted
                coffee, crafted with care by our skilled baristas. Our flexible
                subscription plans ensure that your coffee experience is tailored to
                your needs.
              </p>
            </div>
            <ul className="quality-list">
              <li className="quality-item">
                <picture>
                  <source srcSet="/img/quality/img_05.jpg 1x, /img/quality/img_05@2x.jpg 2x, /img/quality/img_05@3x.jpg 3x" media="(max-width: 767px)" />
                  <img className="quality-image" src="/img/quality/img_05.jpg" alt="Coffee beans" width="288" />
                </picture>
                <h3 className="quality-title-list">Fresh-Roasted Coffee for Every Taste</h3>
                <p className="quality-list-text">Savor the rich flavors of our expertly sourced beans.</p>
              </li>
              <li className="quality-item">
                <picture>
                  <source srcSet="/img/quality/img_04.jpg 1x, /img/quality/img_04@2x.jpg 2x, /img/quality/img_04@3x.jpg 3x" media="(min-width: 768px)" />
                  <img className="quality-image" src="/img/quality/img_04.jpg" alt="Barista" width="288" />
                </picture>
                <h3 className="quality-title-list">Expert Baristas Crafting Your Perfect Brew</h3>
                <p className="quality-list-text">Our baristas are passionate about coffee and skilled in their craft.</p>
              </li>
              <li className="quality-item">
                <picture>
                  <source srcSet="/img/quality/img_03.jpg 1x, /img/quality/img_03@2x.jpg 2x, /img/quality/img_03@3x.jpg 3x" media="(min-width: 1440px)" />
                  <img className="quality-image" src="/img/quality/img_03.jpg" alt="Atmosphere" width="288" />
                </picture>
                <h3 className="quality-title-list">Cozy Atmosphere</h3>
                <p className="quality-list-text">We don’t just serve coffee — we create a warm and welcoming space where every cup feels like home.</p>
              </li>
            </ul>
          </div>
        </section>

        <section className="experience" id="gallery">
          <div className="container">
            <h2 className="experience-title">Coffee Moments</h2>
            <p className="experience-text">Experience the warmth and community of CoffeeShop.</p>
            <ul className="experience-list">
              <li className="experience-item">
                <img className="experience-img" srcSet="/img/experience/img_12.jpg 1x, /img/experience/img_12@2x.jpg 2x" src="/img/experience/img_12.jpg" alt="cup-of-coffee" />
              </li>
              <li className="experience-item">
                <img className="experience-img" srcSet="/img/experience/img_11.jpg 1x, /img/experience/img_11@2x.jpg 2x" src="/img/experience/img_11.jpg" alt="coffee-and-croissant" />
              </li>
              <li className="experience-item">
                <img className="experience-img" srcSet="/img/experience/img_10.jpg 1x, /img/experience/img_10@2x.jpg 2x" src="/img/experience/img_10.jpg" alt="coffee-and-book" />
              </li>
              <li className="experience-item">
                <img className="experience-img" srcSet="/img/experience/img_09.jpg 1x, /img/experience/img_09@2x.jpg 2x" src="/img/experience/img_09.jpg" alt="barista-make-coffee" />
              </li>
              <li className="experience-item">
                <img className="experience-img" srcSet="/img/experience/img_08.jpg 1x, /img/experience/img_08@2x.jpg 2x" src="/img/experience/img_08.jpg" alt="cafe" />
              </li>
              <li className="experience-item">
                <img className="experience-img" srcSet="/img/experience/img_07.jpg 1x, /img/experience/img_07@2x.jpg 2x" src="/img/experience/img_07.jpg" alt="two-cups-of-coffee" />
              </li>
              <li className="experience-item">
                <img className="experience-img last-img" srcSet="/img/experience/img_06.jpg 1x, /img/experience/img_06@2x.jpg 2x" src="/img/experience/img_06.jpg" alt="coffee-to-go" />
              </li>
            </ul>
          </div>
        </section>

        <section className="subscribes">
          <div className="container subscribe_f">
            <div className="sub_t_d_desk">
              <h2 className="subscribe_title">Join Our Coffee Community</h2>
              <p className="subscribe_describe">Experience the joy of coffee delivered right to your door. Sign up today!</p>
            </div>
            <div>
              <form className="input_form" name="subscribe_form" autoComplete="on">
                <label className="input_label" htmlFor="input-email" style={{
                    position: 'absolute',
                    width: '1px',
                    height: '1px',
                    overflow: 'hidden',
                    clip: 'rect(1px, 1px, 1px, 1px)'
                  }}>
                  Your Email Here
                </label>
                <input id="input-email" className="input_item" name="input-email" type="email" placeholder="Your Email Here" required />
                <button className="input_button" type="submit">
                  Subscribe
                  <svg className="input_icon" width="24" height="24">
                    <use href="/icons.svg#icon-chevrons-right"></use>
                  </svg>
                </button>
              </form>
              <p className="subscribe_explanation">By clicking Get Started, you agree to our Terms and Conditions.</p>
            </div>
            <img className="img_width_height" src="/img/subscribe/img_13.jpg" srcSet="/img/subscribe/img_13.jpg 1x, /img/subscribe/img_13@2x.jpg 2x, /img/subscribe/img_13@3x.jpg 3x" alt="Join Our Coffee Community" />
          </div>
        </section>

        <section id="testimonials" className="test_container">
          <div className="container">
            <h2 className="test_title">What our visitors say</h2>
            <ul className="testimon_wrapper">
              <li className="observation">
                <ul className="star-wrapper">
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="outline_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-outline" /></svg></li>
                </ul>
                <p className="test_text">"Every cup feels like a warm hug! I’m addicted to their seasonal flavors!"</p>
                <cite className="sign_text">Name Surname</cite>
              </li>

              <li className="observation">
                <ul className="star-wrapper">
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="outline_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-outline" /></svg></li>
                </ul>
                <p className="test_text">
                  "CoffeeShop is my go-to place for a perfect cup of coffee! The ambiance is so inviting, and the baristas are always friendly."
                </p>
                <cite className="sign_text">Alice Johnson</cite>
              </li>

              <li className="observation">
                <ul className="star-wrapper">
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                </ul>
                <p className="test_text">
                  "I love the variety of blends offered at CoffeeJoy. Every visit feels like a new experience!"
                </p>
                <cite className="sign_text">Michael Smith</cite>
              </li>

              <li className="observation">
                <ul className="star-wrapper">
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="outline_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-outline" /></svg></li>
                  <li><svg className="outline_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-outline" /></svg></li>
                </ul>
                <p className="test_text">
                  "The pastries are to die for! I can’t resist pairing them with my favorite latte. Highly recommend this spot!"
                </p>
                <cite className="sign_text">Emily Davis</cite>
              </li>

              <li className="observation">
                <ul className="star-wrapper">
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                </ul>
                <p className="test_text">
                  "A cozy little gem! CoffeeJoy has the best cold brew in town. I come here every weekend!"
                </p>
                <cite className="sign_text">James Brown</cite>
              </li>

              <li className="observation">
                <ul className="star-wrapper">
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="outline_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-outline" /></svg></li>
                  <li><svg className="outline_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-outline" /></svg></li>
                </ul>
                <p className="test_text">
                  "The atmosphere is perfect for working or relaxing with friends. Their Wi-Fi is fast too!"
                </p>
                <cite className="sign_text">Sarah Wilson</cite>
              </li>

              <li className="observation">
                <ul className="star-wrapper">
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="outline_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-outline" /></svg></li>
                </ul>
                <p className="test_text">
                  "I’m addicted to their seasonal flavors! Each one is crafted with such care and creativity."
                </p>
                <cite className="sign_text">Chris Lee</cite>
              </li>

              <li className="observation">
                <ul className="star-wrapper">
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="filled_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-filled" /></svg></li>
                  <li><svg className="outline_star_icon" width="19" height="20"><use href="/icons.svg#icon-star-outline" /></svg></li>
                </ul>
                <p className="test_text">
                  "I appreciate how they source their coffee beans sustainably. It's great to support such an ethical business!"
                </p>
                <cite className="sign_text">Patricia Taylor</cite>
              </li>
            </ul>
          </div>
        </section>

        <section className="location" id="location">
          <div className="container location-container">
            <div className="location-info">
              <p className="location-dir">Find</p>
              <h2 className="location-header">Location</h2>
              <p className="location-text">Visit us at our cozy coffee shop!</p>
              <ul className="location-contacts">
                <li className="location-contacts-item">
                  <svg className="location-item-icon" width="32" height="32">
                    <use href="/icons.svg#icon-envelope"></use>
                  </svg>
                  <h3 className="location-contacts-header">Email</h3>
                  <a className="location-contacts-link" href="mailto:hello@CoffeeShop.com">hello@CoffeeShop.com</a>
                </li>
                <li className="location-contacts-item">
                  <svg className="location-item-icon" width="32" height="32">
                    <use href="/icons.svg#icon-phone"></use>
                  </svg>
                  <h3 className="location-contacts-header">Phone</h3>
                  <a className="location-contacts-link" href="tel:+15551235321">+1 (555) 123-5321</a>
                </li>
                <li className="location-contacts-item">
                  <svg className="location-item-icon" width="32" height="32">
                    <use href="/icons.svg#icon-map"></use>
                  </svg>
                  <h3 className="location-contacts-header">Office</h3>
                  <p className="location-address">263 Newbury St, Boston, MA 02116, USA</p>
                  <a className="location-get-directions-link" href="https://maps.app.goo.gl/DKZNFEHuvyGGJvJKA" target="_blank" rel="noreferrer">
                    <p className="location-get-directions-text">Get Directions</p>
                    <svg className="location-direction-icon" width="24" height="24">
                      <use href="/icons.svg#icon-chevron-right"></use>
                    </svg>
                  </a>
                </li>
              </ul>
            </div>
            <div className="map-container">
              <iframe 
                src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2948.668927640884!2d-71.08594152371518!3d42.34958143576317!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x89e37a0f084cda31%3A0x2c61033530218739!2zMjYzIE5ld2J1cnkgU3QsIEJvc3RvbiwgTUEgMDIxMTYsINCh0KjQkA!5e0!3m2!1sru!2sua!4v1762371705039!5m2!1sru!2sua" 
                width="600" 
                height="450" 
                style={{ border: 0 }} 
                allowFullScreen="" 
                loading="lazy" 
                referrerPolicy="no-referrer-when-downgrade">
              </iframe>
            </div>
          </div>
        </section>
      </main>

      <Footer />

      {/* MOBILE MENU */}
      <div className="container mobile-menu" data-menu="">
        <div className="mobile-menu-logo-btn">
          <a className="logo" href="/">
            <svg className="icon-vector" width="84" height="36">
              <use href="/icons.svg#icon-company-logo"></use>
            </svg>
          </a>
          <button className="mobile-menu-close" type="button" data-menu-close="">
            <svg className="icon-vector-mobile-menu" width="24" height="24">
              <use href="/icons.svg#icon-x"></use>
            </svg>
          </button>
        </div>
        <nav className="mobile-menu-nav">
          <ul className="mobile-menu-list">
            <li className="mobile-menu-nav-item"><a className="mobile-menu-link" href="#welcome">About Us</a></li>
            <li className="mobile-menu-nav-item"><a className="mobile-menu-link" href="/catalog">Catalog</a></li>
            <li className="mobile-menu-nav-item"><a className="mobile-menu-link" href="#quality">Quality</a></li>
            <li className="mobile-menu-nav-item"><a className="mobile-menu-link" href="#testimonials">Testimonials</a></li>
            <li className="mobile-menu-nav-item"><a className="mobile-menu-link" href="#location">Contacts</a></li>
          </ul>
        </nav>
      </div>

    </>
  );
};

export default Home;