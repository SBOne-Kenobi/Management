#include "Bank.h"

namespace Management {
  Bank::Bank(int N) : price_level(2), gen(std::random_device().operator()()) {
    probOfPriceChange = {
            {1. / 3,  1. / 3,  1. / 6, 1. / 12, 1. / 12},
            {1. / 4,  1. / 3,  1. / 4, 1. / 12, 1. / 12},
            {1. / 12, 1. / 4,  1. / 3, 1. / 4,  1. / 12},
            {1. / 12, 1. / 12, 1. / 4, 1. / 3,  1. / 4},
            {1. / 12, 1. / 12, 1. / 6, 1. / 3,  1. / 3}
    };
    priceLevelInfo = {
            {1.0, 800, 3.0, 6500},
            {1.5, 650, 2.5, 6000},
            {2.0, 500, 2.0, 5500},
            {2.5, 400, 1.5, 5000},
            {3.0, 300, 1.0, 4500}
    };
    materials = floor(N * priceLevelInfo[price_level].UMat);
    product = floor(N * priceLevelInfo[price_level].UProd);
  }

  int Bank::getPriceLevel() const {
    return price_level + 1;
  }

  void Bank::setNewPriceLevel(int N) {
    price_level = probOfPriceChange[price_level](gen);
    materials = floor(N * priceLevelInfo[price_level].UMat);
    product = floor(N * priceLevelInfo[price_level].UProd);
  }

  DemandOffer Bank::getInfo() const {
    return priceLevelInfo[price_level];
  }

  std::vector<int> Bank::requestOfMat(const std::vector<Demand> &demand) {
    std::vector<int> accepted;
    std::vector<int> ord(demand.size());
    for (int i = 0; i < (int) demand.size(); i++)
      ord[i] = i;
    std::sort(ord.begin(), ord.end(), [&demand](int a, int b) { return demand[a] < demand[b]; });
    for (auto i : ord) {
      if (demand[i].Price < getInfo().MinPrice)
        break;
      if (demand[i].UMat > materials)
        continue;
      materials -= demand[i].UMat;
      accepted.push_back(i);
    }
    return accepted;
  }

  std::vector<int> Bank::requestOfProd(const std::vector<Offer> &offer) {
    std::vector<int> accepted;
    std::vector<int> ord(offer.size());
    for (int i = 0; i < (int) offer.size(); i++)
      ord[i] = i;
    std::sort(ord.begin(), ord.end(), [&offer](int a, int b) { return offer[a] < offer[b]; });
    for (auto i : ord) {
      if (offer[i].Price > getInfo().MaxPrice)
        break;
      if (offer[i].UProd > product)
        continue;
      product -= offer[i].UProd;
      accepted.push_back(i);
    }
    return accepted;
  }

  bool Demand::operator<(const Demand &x) const {
    return Price > x.Price || (Price == x.Price && Priority < x.Priority);
  }

  bool Offer::operator<(const Offer &x) const {
    return Price < x.Price || (Price == x.Price && Priority < x.Priority);
  }
}