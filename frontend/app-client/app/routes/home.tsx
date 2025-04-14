import React, { useState, useEffect, useRef } from "react";
import {
  LayoutDashboard,
  FolderKanban,
  Menu,
  LogOut,
  ChevronLeft,
  ChevronRight,
  Palette,
  X,
  User,
  ChartAreaIcon,
  ChartBarIcon,
  Settings,
  ChartArea,
  ChartColumnBigIcon,
} from "lucide-react";
import {
  Link,
  useLocation,
  useMatch,
  useNavigate,
  redirect,
  type LoaderFunctionArgs,
  Outlet,
  useLoaderData,
  Form
} from "react-router";
import APP_ROUTES from "~/utils/appRoutes";
import ThemeSelector from "~/components/themeSelector";

export async function loader({ request }: LoaderFunctionArgs) {


  return {}
}

// Tipos para los componentes
type NavItemProps = {
  to: string;
  icon: React.ReactNode;
  label: string;
  isExpanded: boolean;
};

// Section header for sidebar categories
const SectionHeader: React.FC<{ label: string; isExpanded: boolean }> = ({
  label,
  isExpanded,
}) => {
  if (!isExpanded) return null;

  return (
    <div className="px-3 py-2 mt-2 mb-1">
      <span className="text-xs font-semibold text-base-content/60 uppercase tracking-wider">
        {label}
      </span>
    </div>
  );
};

// NavItem component with React Router compatible active state handling
const NavItem: React.FC<NavItemProps> = ({ to, icon, label, isExpanded }) => {
  const match = useMatch(to === "/" ? "/" : `${to}/*`);
  const isActive = !!match;

  return (
    <Link
      to={to}
      className={`flex items-center gap-3 px-3 py-2 rounded-lg transition-colors hover:bg-base-300 ${isActive ? "bg-primary text-primary-content" : "text-base-content"
        } ${!isExpanded ? "justify-center" : ""}`}
    >
      <div className="flex-shrink-0">{icon}</div>
      {isExpanded && (
        <span className="whitespace-nowrap overflow-hidden transition-all">
          {label}
        </span>
      )}
    </Link>
  );
};

// Componente de la página principal que contiene AppLayout
export default function HomePage() {


  const [isExpanded, setIsExpanded] = useState<boolean>(true);
  const [isMobile, setIsMobile] = useState<boolean>(false);
  const [isSidebarOpen, setIsSidebarOpen] = useState<boolean>(false);
  const [isMobileThemeMenuOpen, setIsMobileThemeMenuOpen] = useState<boolean>(false);
  const location = useLocation();
  const navigate = useNavigate();
  const mobileThemeMenuRef = useRef<HTMLDivElement>(null);


  // Close sidebar on route change in mobile view
  useEffect(() => {
    if (isMobile) {
      setIsSidebarOpen(false);
    }
  }, [location, isMobile]);

  // Check if mobile view
  useEffect(() => {
    const checkIfMobile = () => {
      const isMobileView = window.innerWidth < 1024;
      setIsMobile(isMobileView);
      if (isMobileView) {
        setIsExpanded(true); // Always expanded in mobile drawer
        setIsSidebarOpen(false); // Initially closed on mobile
      } else {
        setIsExpanded(true); // Default to expanded on desktop
      }
    };

    checkIfMobile();
    window.addEventListener("resize", checkIfMobile);

    return () => {
      window.removeEventListener("resize", checkIfMobile);
    };
  }, []);

  // Handle click outside mobile theme menu
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (
        mobileThemeMenuRef.current &&
        !mobileThemeMenuRef.current.contains(event.target as Node)
      ) {
        setIsMobileThemeMenuOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  // Toggle sidebar expansion
  const toggleSidebar = () => {
    if (isMobile) {
      setIsSidebarOpen(!isSidebarOpen);
    } else {
      setIsExpanded(!isExpanded);
    }
  };

  return (
    <div className="flex flex-col h-screen bg-base-200">
      {/* Mobile navbar */}
      <div className="navbar bg-base-100 lg:hidden sticky top-0 z-30 shadow-sm">
        <div className="flex-1">
          <button className="btn btn-ghost btn-circle" onClick={toggleSidebar}>
            <Menu size={20} />
          </button>
          <span className="text-lg font-bold ml-2">
            App Name
          </span>
        </div>
        <div className="flex-none flex gap-2">
          <div className="dropdown dropdown-end">
            <div tabIndex={0} role="button" className="btn btn-ghost btn-circle avatar">
              <div className="w-8 h-8 rounded-full bg-neutral-focus text-neutral-content grid place-items-center">
                <span className="text-xs font-bold">U</span>
              </div>
            </div>
            <ul tabIndex={0} className="mt-3 z-[1] p-2 shadow menu menu-sm dropdown-content bg-base-100 rounded-box w-52">
              <li className="p-2 border-b border-base-200">
                <div className="flex flex-col">
                  <span className="font-medium">
                    User Name
                  </span>
                  <span className="text-xs opacity-70 truncate">
                    user@email.com
                  </span>
                </div>
              </li>
              <li>
                <button className="justify-between">
                  Profile
                </button>
              </li>
              <li>
                <button
                  onClick={() => setIsMobileThemeMenuOpen(!isMobileThemeMenuOpen)}
                  className="justify-between"
                >
                  Theme <Palette size={16} />
                </button>
              </li>
              <li>
                <Form method="post">
                  <input type="hidden" name="actionType" value="logout" />
                  <button type="submit" className="text-error justify-between">
                    Logout <LogOut size={16} />
                  </button>
                </Form>
              </li>
            </ul>
          </div>
        </div>
      </div>

      <div className="flex flex-1 overflow-hidden">
        {/* Overlay for mobile */}
        {isMobile && isSidebarOpen && (
          <div
            className="fixed inset-0 bg-black bg-opacity-50 z-20 lg:hidden"
            onClick={() => setIsSidebarOpen(false)}
          ></div>
        )}

        {/* Sidebar */}
        <aside
          className={`
            h-screen bg-base-100 shadow-lg flex flex-col
            fixed lg:static top-0 z-30 transition-all duration-300
            ${isMobile ? (isSidebarOpen ? "left-0" : "-left-full") : "left-0"}
            ${isExpanded ? "lg:w-64" : "lg:w-20"}
            ${isMobile ? "w-3/4 max-w-xs" : ""}
          `}
        >
          {/* Header */}
          <div className="p-4 flex items-center justify-between border-b border-base-300">
            <div className="flex items-center">
              <div className="avatar">
                <div className="w-10 rounded-full bg-primary text-primary-content grid place-items-center">
                  <span className="text-xl font-bold">A</span>
                </div>
              </div>
              {(isExpanded || isMobile) && (
                <div className="ml-3">
                  <span className="text-xl font-semibold whitespace-nowrap overflow-hidden transition-all">
                    App Name
                  </span>

                </div>
              )}
            </div>

            {/* Toggle button for desktop */}
            {!isMobile && (
              <button
                className="btn btn-sm btn-ghost btn-circle"
                onClick={toggleSidebar}
              >
                {isExpanded ? (
                  <ChevronLeft size={18} />
                ) : (
                  <ChevronRight size={18} />
                )}
              </button>
            )}

            {/* Close button for mobile */}
            {isMobile && (
              <button
                className="btn btn-sm btn-ghost"
                onClick={() => setIsSidebarOpen(false)}
              >
                <X size={18} />
              </button>
            )}
          </div>

          {/* Navigation Links */}
          <div className="flex-1 overflow-y-auto py-4 px-3 flex flex-col gap-1">
            {/* Main Navigation */}
            <SectionHeader label="Main" isExpanded={isExpanded || isMobile} />
            <NavItem
              to={APP_ROUTES.HOME.OVERVIEW}
              icon={<LayoutDashboard size={20} />}
              label="Overview"
              isExpanded={isExpanded || isMobile}
            />
          </div>

          {/* Footer Section - only show on desktop or when mobile sidebar is open */}
          <div className="p-4 border-t border-base-300">
            {/* Theme selector - mobile optimized */}
            {!isMobile && <ThemeSelector isExpanded={isExpanded} />}

            {/* User info and logout - mobile optimized */}
            {(!isMobile || (isMobile && isSidebarOpen)) && (
              <div className="flex flex-col gap-4 mt-4">
                <div className="flex items-center gap-3">
                  <div className="avatar">
                    <div className="w-8 h-8 rounded-full bg-neutral-focus text-neutral-content grid place-items-center">
                      <span className="text-xs font-bold">U</span>
                    </div>
                  </div>

                </div>
                {!isMobile && (
                  <LogOut size={16} />
                )}
              </div>
            )}
          </div>
        </aside>

        {/* Main Content */}
        <main className="flex-1 overflow-auto p-2 lg:p-4">
          <div className="container mx-auto">
            <Outlet />
          </div>
        </main>
      </div>

      {/* Mobile Theme Menu (shown as a modal) */}
      {isMobileThemeMenuOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-40">
          <div
            className="bg-base-100 rounded-lg shadow-xl p-4 mx-4 w-full max-w-sm"
            ref={mobileThemeMenuRef}
          >
            <div className="flex items-center justify-between px-2 pb-2 border-b border-base-200 mb-3">
              <span className="font-semibold text-lg">Choose Theme</span>
              <button
                className="btn btn-ghost btn-sm btn-circle"
                onClick={() => setIsMobileThemeMenuOpen(false)}
              >
                <X size={16} />
              </button>
            </div>
            <ThemeSelector isExpanded={true} />
            <div className="mt-4 flex justify-end">
              <button
                className="btn btn-primary btn-sm"
                onClick={() => setIsMobileThemeMenuOpen(false)}
              >
                Done
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}