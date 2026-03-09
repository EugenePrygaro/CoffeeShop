/**
 * @param {Array} products - Масив об'єктів товарів [ {id, name, price, desc}, ... ]
 * @returns {Array} - Масив об'єктів з доданим полем img
 */

const products = [
  {
    id: 1,
    name: 'Ethiopia Sidamo',
    price: 320,
    desc: 'яскраві нотки бергамоту та чорного чаю.',
  },
  {
    id: 2,
    name: 'Colombia Supremo',
    price: 280,
    desc: 'класичний смак з відтінками карамелі та горіхів.',
  },
  {
    id: 3,
    name: 'Brazil Santos',
    price: 240,
    desc: 'насичена кава з низькою кислотністю та шоколадним післясмаком.',
  },
  {
    id: 4,
    name: 'Kenya AA',
    price: 450,
    desc: 'ексклюзивний сорт з винним ароматом та ягідним смаком.',
  },
  {
    id: 5,
    name: 'Guatemala Antigua',
    price: 310,
    desc: 'димні нотки та глибокий смак какао.',
  },
  {
    id: 6,
    name: 'India Monsooned Malabar',
    price: 290,
    desc: 'унікальна кава з мускусними та пряними відтінками.',
  },
  {
    id: 7,
    name: 'Costa Rica Tarrazu',
    price: 340,
    desc: 'чиста чашка з фруктовим ароматом.',
  },
  {
    id: 8,
    name: 'Vietnam Robusta',
    price: 180,
    desc: 'міцна кава з високим вмістом кофеїну.',
  },
  {
    id: 9,
    name: 'Oolong Tea',
    price: 210,
    desc: 'традиційний китайський чай з ніжним молочним ароматом.',
  },
  {
    id: 10,
    name: 'Earl Grey',
    price: 150,
    desc: 'чорний чай з додаванням олії бергамоту.',
  },
  {
    id: 11,
    name: 'Green Jasmine',
    price: 190,
    desc: 'зелений чай з ніжними пелюстками жасмину.',
  },
  {
    id: 12,
    name: 'Pu-erh Royal',
    price: 550,
    desc: 'витриманий чай з глибоким земляним смаком.',
  },
  {
    id: 13,
    name: 'Rwanda Bourbon',
    price: 380,
    desc: 'солодкий профіль з нотками меду.',
  },
  {
    id: 14,
    name: 'Sumatra Mandheling',
    price: 330,
    desc: 'низька кислотність та щільне тіло.',
  },
  {
    id: 15,
    name: 'Paper Filters',
    price: 120,
    desc: 'набір з 100 паперових фільтрів для воронки.',
  },
  {
    id: 16,
    name: 'Ceramic V60',
    price: 850,
    desc: 'класична керамічна воронка для ручного заварювання.',
  },
  {
    id: 17,
    name: 'Burr Grinder',
    price: 2400,
    desc: 'ручний млинок з керамічними жорнами.',
  },
  {
    id: 18,
    name: 'French Press',
    price: 400,
    desc: 'простий спосіб заварювання насиченої кави.',
  },
  {
    id: 19,
    name: 'Decaf Coffee',
    price: 300,
    desc: '100% арабіка без кофеїну.',
  },
  {
    id: 20,
    name: 'Specialty Blend',
    price: 420,
    desc: 'авторська суміш від нашого обсмажувальника.',
  },
];

export const richProductsWithImg = () => {
  return products.map((product, index) => {
    const searchTerm = product.name
      .toLowerCase()
      .replace(/[^a-zа-яєіїґ\s]/gi, '')
      .trim()
      .replace(/\s+/g, ',');

    return {
      ...product,
      img: `https://loremflickr.com/600/400/coffee,${searchTerm}/all?lock=${product.id}`,
    };
  });
};
