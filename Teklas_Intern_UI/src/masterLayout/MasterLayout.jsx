import React, { useEffect, useState } from "react";
import { Icon } from "@iconify/react/dist/iconify.js";
import { Link, NavLink, useLocation, useNavigate } from "react-router-dom";
import { useAuth } from "../contexts/AuthContext";
import PermissionGuard from "../components/PermissionGuard";
import { usePermissions } from "../hooks/usePermissions";
import { PERMISSIONS } from "../utils/permissions";
import ThemeToggleButton from "../helper/ThemeToggleButton";
import Swal from 'sweetalert2';

const MasterLayout = ({ children }) => {
  let [sidebarActive, seSidebarActive] = useState(false);
  let [mobileMenu, setMobileMenu] = useState(false);
  const location = useLocation();
  const navigate = useNavigate();
  
  // Auth context
  const { user, logout, isAuthenticated } = useAuth();
  const permissions = usePermissions();

  // Redirect if not authenticated
  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/sign-in', { replace: true });
    }
  }, [isAuthenticated, navigate]);

  useEffect(() => {
    const handleDropdownClick = (event) => {
      event.preventDefault();
      const clickedLink = event.currentTarget;
      const clickedDropdown = clickedLink.closest(".dropdown");

      if (!clickedDropdown) return;

      const isActive = clickedDropdown.classList.contains("open");

      // Close all dropdowns
      const allDropdowns = document.querySelectorAll(".sidebar-menu .dropdown");
      allDropdowns.forEach((dropdown) => {
        dropdown.classList.remove("open");
        const submenu = dropdown.querySelector(".sidebar-submenu");
        if (submenu) {
          submenu.style.maxHeight = "0px"; // Collapse submenu
        }
      });

      // Toggle the clicked dropdown
      if (!isActive) {
        clickedDropdown.classList.add("open");
        const submenu = clickedDropdown.querySelector(".sidebar-submenu");
        if (submenu) {
          submenu.style.maxHeight = `${submenu.scrollHeight}px`; // Expand submenu
        }
      }
    };

    // Attach click event listeners to all dropdown triggers
    const dropdownTriggers = document.querySelectorAll(
      ".sidebar-menu .dropdown > a, .sidebar-menu .dropdown > Link"
    );

    dropdownTriggers.forEach((trigger) => {
      trigger.addEventListener("click", handleDropdownClick);
    });

    const openActiveDropdown = () => {
      const allDropdowns = document.querySelectorAll(".sidebar-menu .dropdown");
      allDropdowns.forEach((dropdown) => {
        const submenuLinks = dropdown.querySelectorAll(".sidebar-submenu li a");
        submenuLinks.forEach((link) => {
          if (
            link.getAttribute("href") === location.pathname ||
            link.getAttribute("to") === location.pathname
          ) {
            dropdown.classList.add("open");
            const submenu = dropdown.querySelector(".sidebar-submenu");
            if (submenu) {
              submenu.style.maxHeight = `${submenu.scrollHeight}px`; // Expand submenu
            }
          }
        });
      });
    };

    // Open the submenu that contains the active route
    openActiveDropdown();

    // Cleanup event listeners on unmount
    return () => {
      dropdownTriggers.forEach((trigger) => {
        trigger.removeEventListener("click", handleDropdownClick);
      });
    };
  }, [location.pathname]);

  let sidebarControl = () => {
    seSidebarActive(!sidebarActive);
  };

  let mobileMenuControl = () => {
    setMobileMenu(!mobileMenu);
  };

  const handleLogout = async () => {
    const result = await Swal.fire({
      title: 'Çıkış Yap',
      text: 'Çıkış yapmak istediğinizden emin misiniz?',
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Evet, Çıkış Yap',
      cancelButtonText: 'İptal',
      confirmButtonColor: '#dc3545',
      cancelButtonColor: '#6c757d'
    });

    if (result.isConfirmed) {
      await logout();
      navigate('/sign-in', { replace: true });
    }
  };

  // Get user display name
  const getUserDisplayName = () => {
    if (!user) return 'User';
    return user.firstName && user.lastName 
      ? `${user.firstName} ${user.lastName}`
      : user.username || user.email || 'User';
  };

  // Get user role display
  const getUserRoleDisplay = () => {
    if (!user?.roleNames || user.roleNames.length === 0) return 'User';
    return user.roleNames.join(', ');
  };

  return (
    <section className={mobileMenu ? "overlay active" : "overlay "}>
      {/* sidebar */}
      <aside
        className={
          sidebarActive
            ? "sidebar active "
            : mobileMenu
            ? "sidebar sidebar-open"
            : "sidebar"
        }
      >
        <button
          onClick={mobileMenuControl}
          type='button'
          className='sidebar-close-btn'
        >
          <Icon icon='radix-icons:cross-2' />
        </button>
        <div>
          <Link to='/' className='sidebar-logo'>
            <img
              src='assets/images/logo.svg'
              alt='site logo'
              className='light-logo'
            />
            <img
              src='assets/images/logo-light.svg'
              alt='site logo'
              className='dark-logo'
            />
            <img
              src='assets/images/logo-icon.png'
              alt='site logo'
              className='logo-icon'
            />
          </Link>
        </div>
        <div className='sidebar-menu-area'>
          <ul className='sidebar-menu' id='sidebar-menu'>
            {/* Ana Sayfa - Direkt buton */}
            <li>
              <NavLink to='/' className={(navData) => navData.isActive ? "active-page" : ""}>
                <Icon icon='solar:home-smile-angle-outline' className='menu-icon' />
                <span>Ana Sayfa</span>
              </NavLink>
            </li>

            {/* Malzeme Yönetimi */}
            <PermissionGuard permission={PERMISSIONS.MATERIALS_VIEW}>
            <li className='dropdown'>
              <a href='#'>
                <Icon icon='mdi:package-variant' className='menu-icon' />
                <span>Malzeme Yönetimi</span>
              </a>
              <ul className='sidebar-submenu'>
                <li>
                  <NavLink to='/material-cards' className={(navData) => navData.isActive ? 'active-page' : ''}>
                    <span>Malzeme Kartları</span>
                  </NavLink>
                </li>
                <li>
                  <NavLink to='/material-category' className={(navData) => navData.isActive ? 'active-page' : ''}>
                    <span>Kategori Tanımlama</span>
                  </NavLink>
                </li>
                <li>
                  <NavLink to='/material-movements' className={(navData) => navData.isActive ? 'active-page' : ''}>
                    <span>Malzeme Hareketleri</span>
                  </NavLink>
                </li>
                  <PermissionGuard permission={PERMISSIONS.MATERIALS_VIEW}>
                    <li>
                      <NavLink to='/material-card-trash' className={(navData) => navData.isActive ? 'active-page' : ''}>
                        <span>Malzeme Kartları (Çöp Kutusu)</span>
                      </NavLink>
                    </li>
                  </PermissionGuard>
                  <PermissionGuard permission={PERMISSIONS.CATEGORIES_VIEW}>
                    <li>
                      <NavLink to='/material-category-trash' className={(navData) => navData.isActive ? 'active-page' : ''}>
                        <span>Kategori (Çöp Kutusu)</span>
                      </NavLink>
                    </li>
                  </PermissionGuard>
                  <PermissionGuard permission={PERMISSIONS.MOVEMENTS_VIEW}>
                    <li>
                      <NavLink to='/material-movement-trash' className={(navData) => navData.isActive ? 'active-page' : ''}>
                        <span>Malzeme Hareketleri (Çöp Kutusu)</span>
                      </NavLink>
                    </li>
                  </PermissionGuard>
              </ul>
            </li>
            </PermissionGuard>

            {/* Üretim Yönetimi */}
            <PermissionGuard permission={PERMISSIONS.PRODUCTION_VIEW}>
            <li className='dropdown'>
              <a href='#'>
                <Icon icon='mdi:factory' className='menu-icon' />
                <span>Üretim Yönetimi</span>
              </a>
              <ul className='sidebar-submenu'>
                  <li>
                    <NavLink to='/work-orders' className={(navData) => navData.isActive ? 'active-page' : ''}>
                      <span>İş Emirleri</span>
                    </NavLink>
                  </li>
                  <li>
                    <NavLink to='/production-confirmations' className={(navData) => navData.isActive ? 'active-page' : ''}>
                      <span>Üretim Onayları</span>
                    </NavLink>
                  </li>
                  <li>
                    <NavLink to='/bill-of-materials' className={(navData) => navData.isActive ? 'active-page' : ''}>
                      <span>Reçeteler</span>
                    </NavLink>
                  </li>
              </ul>
            </li>
            </PermissionGuard>

            {/* Satınalma Yönetimi - Placeholder */}
            <li className='dropdown'>
              <a href='#'>
                <Icon icon='mdi:cart-outline' className='menu-icon' />
                <span>Satınalma Yönetimi</span>
              </a>
              <ul className='sidebar-submenu'>
                <li>
                  <span className='px-3 py-2 text-muted'>Yakında...</span>
                </li>
              </ul>
            </li>

            {/* Satış Yönetimi - Placeholder */}
            <li className='dropdown'>
              <a href='#'>
                <Icon icon='mdi:account-group-outline' className='menu-icon' />
                <span>Satış Yönetimi</span>
              </a>
              <ul className='sidebar-submenu'>
                <li>
                  <span className='px-3 py-2 text-muted'>Yakında...</span>
                </li>
              </ul>
            </li>

            {/* Depo/Stok Yönetimi - Placeholder */}
            <li className='dropdown'>
              <a href='#'>
                <Icon icon='mdi:warehouse' className='menu-icon' />
                <span>Depo/Stok Yönetimi</span>
              </a>
              <ul className='sidebar-submenu'>
                <li>
                  <span className='px-3 py-2 text-muted'>Yakında...</span>
                </li>
              </ul>
            </li>

            {/* Raporlar */}
            <PermissionGuard permission={PERMISSIONS.REPORTS_VIEW}>
              <li className='dropdown'>
                <a href='#'>
                  <Icon icon='mdi:chart-line' className='menu-icon' />
                  <span>Raporlar</span>
                </a>
                <ul className='sidebar-submenu'>
                  <li>
                    <NavLink to='/reports/materials' className={(navData) => navData.isActive ? 'active-page' : ''}>
                      <span>Malzeme Raporları</span>
                    </NavLink>
                  </li>
                  <li>
                    <NavLink to='/reports/production' className={(navData) => navData.isActive ? 'active-page' : ''}>
                      <span>Üretim Raporları</span>
                    </NavLink>
                  </li>
                  <li>
                    <NavLink to='/reports/movements' className={(navData) => navData.isActive ? 'active-page' : ''}>
                      <span>Hareket Raporları</span>
                    </NavLink>
                  </li>
                </ul>
              </li>
            </PermissionGuard>

            {/* Kullanıcı Yönetimi - Sadece Admin */}
            <PermissionGuard adminOnly={true}>
              <li className='dropdown'>
                <a href='#'>
                  <Icon icon='mdi:account-group' className='menu-icon' />
                  <span>Kullanıcı Yönetimi</span>
                </a>
                <ul className='sidebar-submenu'>
                  <li>
                    <NavLink to='/users-list' className={(navData) => navData.isActive ? 'active-page' : ''}>
                      <span>Kullanıcılar</span>
                    </NavLink>
                  </li>
                  <li>
                    <NavLink to='/add-user' className={(navData) => navData.isActive ? 'active-page' : ''}>
                      <span>Kullanıcı Ekle</span>
                    </NavLink>
                  </li>
                  <li>
                    <NavLink to='/assign-role' className={(navData) => navData.isActive ? 'active-page' : ''}>
                      <span>Rol Atama</span>
                    </NavLink>
                  </li>
                </ul>
              </li>
            </PermissionGuard>

            {/* Sistem Ayarları */}
            <PermissionGuard permission={PERMISSIONS.SETTINGS_VIEW}>
              <li className='dropdown'>
                <a href='#'>
                  <Icon icon='mdi:cog' className='menu-icon' />
                  <span>Sistem Ayarları</span>
                </a>
                <ul className='sidebar-submenu'>
                  <li>
                    <NavLink to='/settings' className={(navData) => navData.isActive ? 'active-page' : ''}>
                      <span>Genel Ayarlar</span>
                    </NavLink>
                  </li>
                  <PermissionGuard adminOnly={true}>
                    <li>
                      <NavLink to='/settings/system' className={(navData) => navData.isActive ? 'active-page' : ''}>
                        <span>Sistem Ayarları</span>
                      </NavLink>
                    </li>
                  </PermissionGuard>
                </ul>
              </li>
            </PermissionGuard>
          </ul>
        </div>
      </aside>

      <main
        className={sidebarActive ? "dashboard-main active" : "dashboard-main"}
      >
        <div className='navbar-header'>
          <div className='row align-items-center justify-content-between'>
            <div className='col-auto'>
              <div className='d-flex flex-wrap align-items-center gap-4'>
                <button
                  type='button'
                  className='sidebar-toggle'
                  onClick={sidebarControl}
                >
                  {sidebarActive ? (
                    <Icon
                      icon='iconoir:arrow-right'
                      className='icon text-2xl non-active'
                    />
                  ) : (
                    <Icon
                      icon='heroicons:bars-3-solid'
                      className='icon text-2xl non-active '
                    />
                  )}
                </button>
                <button
                  onClick={mobileMenuControl}
                  type='button'
                  className='sidebar-mobile-toggle'
                >
                  <Icon icon='heroicons:bars-3-solid' className='icon' />
                </button>
                <form className='navbar-search'>
                  <input type='text' name='search' placeholder='Ara...' />
                  <Icon icon='ion:search-outline' className='icon' />
                </form>
              </div>
            </div>
            <div className='col-auto'>
              <div className='d-flex flex-wrap align-items-center gap-3'>
                {/* Theme Toggle */}
                <ThemeToggleButton />
                
                {/* User Role Badge */}
                <div className='d-none d-lg-block'>
                  <span className='badge bg-primary-subtle text-primary'>
                    {getUserRoleDisplay()}
                          </span>
                </div>

                {/* User Profile Dropdown */}
                <div className='dropdown'>
                  <button
                    className='d-flex justify-content-center align-items-center rounded-circle'
                    type='button'
                    data-bs-toggle='dropdown'
                  >
                    <img
                      src={user?.profileImage || 'assets/images/user.png'}
                      alt='user profile'
                      className='w-40-px h-40-px object-fit-cover rounded-circle'
                    />
                  </button>
                  <div className='dropdown-menu to-top dropdown-menu-sm'>
                    <div className='py-12 px-16 radius-8 bg-primary-50 mb-16 d-flex align-items-center justify-content-between gap-2'>
                      <div>
                        <h6 className='text-lg text-primary-light fw-semibold mb-2'>
                          {getUserDisplayName()}
                        </h6>
                        <span className='text-secondary-light fw-medium text-sm'>
                          {getUserRoleDisplay()}
                        </span>
                      </div>
                      <button 
                        type='button' 
                        className='hover-text-danger'
                        onClick={handleLogout}
                      >
                        <Icon
                          icon='radix-icons:cross-1'
                          className='icon text-xl'
                        />
                      </button>
                    </div>
                    <ul className='to-top-list'>
                      <li>
                        <Link
                          className='dropdown-item text-black px-0 py-8 hover-bg-transparent hover-text-primary d-flex align-items-center gap-3'
                          to='/view-profile'
                        >
                          <Icon
                            icon='solar:user-linear'
                            className='icon text-xl'
                          />
                          Profilim
                        </Link>
                      </li>
                      <PermissionGuard adminOnly={true}>
                      <li>
                        <Link
                          className='dropdown-item text-black px-0 py-8 hover-bg-transparent hover-text-primary d-flex align-items-center gap-3'
                            to='/users-list'
                        >
                          <Icon
                              icon='mdi:account-group'
                            className='icon text-xl'
                            />
                            Kullanıcı Yönetimi
                        </Link>
                      </li>
                      </PermissionGuard>
                      <li>
                        <Link
                          className='dropdown-item text-black px-0 py-8 hover-bg-transparent hover-text-primary d-flex align-items-center gap-3'
                          to='/settings'
                        >
                          <Icon
                            icon='icon-park-outline:setting-two'
                            className='icon text-xl'
                          />
                          Ayarlar
                        </Link>
                      </li>
                      <li>
                        <button
                          className='dropdown-item text-black px-0 py-8 hover-bg-transparent hover-text-danger d-flex align-items-center gap-3 border-0 bg-transparent w-100 text-start'
                          onClick={handleLogout}
                        >
                          <Icon icon='lucide:power' className='icon text-xl' />
                          Çıkış Yap
                        </button>
                      </li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        {/* dashboard-main-body */}
        <div className='dashboard-main-body'>{children}</div>
      </main>
    </section>
  );
};

export default MasterLayout;
